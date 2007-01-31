// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

namespace PopulateGames
{
	class Program
	{
		private const string MobyUrl = "http://www.mobygames.com/browse/games/psp/";
		private const string WikiUrl = "http://en.wikipedia.org/wiki/List_of_PlayStation_Portable_games";

		private static Dictionary<string, string> GetMobyList()
		{
			HttpWebRequest req = HttpWebRequest.Create( MobyUrl ) as HttpWebRequest;
			HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
			if( resp == null )
			{
				Console.WriteLine( "Failed to grab URL" );
				return null;
			}

			string html = null;
			using( StreamReader reader = new StreamReader( resp.GetResponseStream() ) )
				html = reader.ReadToEnd();
			if( html == null )
			{
				Console.WriteLine( "Failed to read response from URL" );
				return null;
			}

			int startIndex = html.IndexOf( "<div id=\"browsewindow\" class=\"browseTableList\">" );
			startIndex = html.IndexOf( "<li>", startIndex );
			int endIndex = html.IndexOf( "</ul>", startIndex );
			string cutHtml = html.Substring( startIndex, endIndex - startIndex );

			//<li><a href="/game/psp/archer-macleans-mercury">Archer Maclean's Mercury</a></li>
			Dictionary<string, string> mobyList = new Dictionary<string, string>();
			Regex regex = new Regex( "<li><a\\ href=\"(.+)\">(.+)</a></li>\n", RegexOptions.Multiline );
			foreach( Match match in regex.Matches( cutHtml ) )
			{
				string url = match.Groups[ 1 ].Value;
				string name = match.Groups[ 2 ].Value;

				url = "http://www.mobygames.com" + url;
				mobyList.Add( name, url );
			}

			return mobyList;
		}

		private static Dictionary<string, string> GetWikiList()
		{
			HttpWebRequest req = HttpWebRequest.Create( WikiUrl ) as HttpWebRequest;
			HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
			if( resp == null )
			{
				Console.WriteLine( "Failed to grab URL" );
				return null;
			}

			string html = null;
			using( StreamReader reader = new StreamReader( resp.GetResponseStream() ) )
				html = reader.ReadToEnd();
			if( html == null )
			{
				Console.WriteLine( "Failed to read response from URL" );
				return null;
			}

			int startIndex = html.IndexOf( "<p><a name=\"0-9\"></a></p>" );
			startIndex = html.IndexOf( "<li>", startIndex );
			int endIndex = html.IndexOf( "<p><br /></p>", startIndex );
			string cutHtml = html.Substring( startIndex, endIndex - startIndex );

			//<li><i><a href="/wiki/Naruto:_Narutimate_Portable" title="Naruto: Narutimate Portable">Naruto: Narutimate Portable</a></i> - CyberConnect2</li>
			Dictionary<string, string> wikiList = new Dictionary<string, string>();
			Regex regex = new Regex( "<li><i><a\\ href=\"(.+)\" title=\"(.+)\">.+</a></i>.*</li>\n", RegexOptions.Multiline );
			foreach( Match match in regex.Matches( cutHtml ) )
			{
				string url = match.Groups[ 1 ].Value;
				string name = match.Groups[ 2 ].Value;

				name = name.Replace( "(video game)", "" );
				name = name.Replace( "(computer game)", "" );
				name = name.Replace( "(PSP)", "" );
				name = name.Replace( "(game)", "" );
				name = name.Replace( "(series)", "" );

				if( url.Contains( "action=edit" ) == true )
					url = null;
				else
					url = "http://en.wikipedia.org" + url;
				try
				{
					wikiList.Add( name, url );
				}
				catch
				{
				}
			}

			return wikiList;
		}

		static void Main( string[] args )
		{
			//Dictionary<string, string> list = GetMobyList();
			Dictionary<string, string> list = GetWikiList();

			using( Database db = new Database() )
			{
				string username = ConfigurationManager.AppSettings[ "Username" ];
				string password = ConfigurationManager.AppSettings[ "Password" ];
				db.Authenticate( username, password );

				List<Game> games = db.ListGames();
				List<string> gameNames = new List<string>( games.Count );
				foreach( Game game in games )
					gameNames.Add( game.Title );

				foreach( KeyValuePair<string, string> pair in list )
				{
					if( gameNames.Contains( pair.Key ) == false )
					{
						// Add game
						long gameId = db.AddGame( pair.Key, pair.Value );
					}
				}

				db.Logout();
			}
		}
	}
}
