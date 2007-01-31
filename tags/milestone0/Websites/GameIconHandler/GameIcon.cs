// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace CustomHandlers
{
	public class GameIcon : IHttpHandler, IRequiresSessionState
	{
		public static Regex ExtractIdsRegex = new Regex( "([0-9]+).([0-9]+).gameicon", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline );
		public static List<string> ValidSizes = new List<string>( new string[] { "small" } );
		public static List<string> ValidReferrers = new List<string>( new string[] { "localhost", "dev.ixtli.com", "ixtli.com", "www.ixtli.com", "psp.ixtli.com" } );

		public void ProcessRequest( HttpContext context )
		{
			context.Response.ContentType = "image/png";

			// Referrer check! Kill the baddies!
			string referrerHost = context.Request.UrlReferrer.Host.ToLowerInvariant();
			if( ValidReferrers.Contains( referrerHost ) == false )
			{
				context.Response.WriteFile( context.Server.MapPath( "~/Resources/InvalidIcon.png" ) );
				return;
			}

			UserSession session = UserSession.FromContext( context.Session, true );
			if( session == null )
			{
				context.Response.WriteFile( context.Server.MapPath( "~/Resources/InvalidIcon.png" ) );
				return;
			}

			long gameId;
			long releaseId;
			try
			{
				string filePath = Path.GetFileName( context.Request.FilePath );
				Match match = ExtractIdsRegex.Match( filePath );
				if( ( match == null ) ||
					( match.Success == false ) )
				{
					context.Response.WriteFile( context.Server.MapPath( "~/Resources/InvalidIcon.png" ) );
					return;
				}

				gameId = long.Parse( match.Groups[ 1 ].Value );
				releaseId = long.Parse( match.Groups[ 2 ].Value );

				if( ( gameId <= 0 ) ||
					( releaseId <= 0 ) )
				{
					context.Response.WriteFile( context.Server.MapPath( "~/Resources/InvalidIcon.png" ) );
					return;
				}
			}
			catch
			{
				//throw;
				context.Response.WriteFile( context.Server.MapPath( "~/Resources/InvalidIcon.png" ) );
				return;
			}

			byte[] iconData = session.Database.GetGameIcon( gameId, releaseId );
			if( ( iconData == null ) ||
				( iconData.Length == 0 ) )
			{
				context.Response.WriteFile( context.Server.MapPath( "~/Resources/InvalidIcon.png" ) );
				return;
			}

			context.Response.Cache.SetCacheability( HttpCacheability.Public );
			context.Response.Cache.SetExpires( DateTime.Now.AddMonths( 1 ) );

			string size = context.Request.QueryString[ "size" ].ToLowerInvariant();
			if( ( size != null ) && ( ValidSizes.Contains( size ) == true ) )
			{
				// TODO: Cache this someplace
				try
				{
					using( MemoryStream stream = new MemoryStream( iconData, false ) )
					using( Image source = Image.FromStream( stream ) )
					{
						int width;
						int height;
						switch( size )
						{
							default:
							case "small":
								width = 92;
								height = 51;
								break;
						}

						using( Bitmap dest = new Bitmap( source, width, height ) )
						{

							using( MemoryStream ms = new MemoryStream() )
							{
								dest.Save( ms, ImageFormat.Png );
								ms.WriteTo( context.Response.OutputStream );
							}
						}
					}
				}
				catch( Exception ex )
				{
					Debug.Assert( false, ex.ToString() );
					context.Response.WriteFile( context.Server.MapPath( "~/Resources/InvalidIcon.png" ) );
					return;
				}
			}
			else
				context.Response.OutputStream.Write( iconData, 0, iconData.Length );
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
