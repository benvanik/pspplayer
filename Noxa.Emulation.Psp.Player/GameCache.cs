// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Noxa.Emulation.Psp.Games;
using Noxa.Emulation.Psp.Player.Properties;

namespace Noxa.Emulation.Psp.Player
{
	public class GameCache
	{
		private const string ListFile = "Games.xml";
		private BinaryFormatter _serializer = new BinaryFormatter();
		private string _cachePath;
		private List<GameInformation> _list = new List<GameInformation>();
		private Dictionary<string, GameInformation> _lookup = new Dictionary<string, GameInformation>();

		static GameCache()
		{
			LoadRegionImages();
		}

		public static string DefaultPath
		{
			get
			{
				return Path.Combine( Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Noxa.Emulation.Psp.Player" ), "Cache" );
			}
		}

		public void Load( string cachePath )
		{
			_cachePath = cachePath;

			_list.Clear();
			_lookup.Clear();

			string listFile = Path.Combine( _cachePath, ListFile );
			try
			{
				if( Directory.Exists( _cachePath ) == false )
					Directory.CreateDirectory( _cachePath );
				if( File.Exists( listFile ) == true )
				{
					using( FileStream stream = new FileStream( listFile, FileMode.Open ) )
						_list = ( List<GameInformation> )_serializer.Deserialize( stream );
				}
			}
			catch
			{
				this.Clear();
			}
			if( _list == null )
				_list = new List<GameInformation>();
			else
			{
				foreach( GameInformation game in _list )
				{
					Debug.Assert( game.HostPath != null );
					if( File.Exists( game.HostPath ) == true )
						_lookup.Add( game.HostPath, game );
					else
					{
						Log.WriteLine( Verbosity.Normal, Feature.General, "GameCache::Load ignoring {0} because path not found: {1}", game.Parameters.Title, game.HostPath );
						game.Ignore = true;
					}
				}
			}
		}

		public void Save()
		{
			string listFile = Path.Combine( _cachePath, ListFile );
			try
			{
				if( Directory.Exists( _cachePath ) == false )
					Directory.CreateDirectory( _cachePath );
				using( FileStream stream = new FileStream( listFile, FileMode.Create ) )
					_serializer.Serialize( stream, _list );
			}
			catch
			{
				this.Clear();
			}
		}

		public void Clear()
		{
			_list.Clear();
			try
			{
				Directory.Delete( _cachePath, true );
			}
			catch
			{
			}
		}

		public void Add( GameInformation game )
		{
			Debug.Assert( game.GameType != GameType.Eboot );
			GameInformation copy = ( GameInformation )game.Clone();
			_list.Add( copy );
			_lookup.Add( copy.HostPath, copy );

			// We build the image off the real game, which should have the icon stream
			using( Bitmap b = this.GetImage( game ) )
			{
			}
		}

		public GameInformation this[ string hostPath ]
		{
			get
			{
				GameInformation value;
				if( _lookup.TryGetValue( hostPath, out value ) == false )
					return null;
				else
					return value;
			}
		}

		public void Remove( GameInformation game )
		{
			Debug.Assert( game.HostPath != null );
			this.Remove( game.HostPath );
		}

		public void Remove( string hostPath )
		{
			GameInformation local = _lookup[ hostPath ];
			Debug.Assert( local != null );
			_lookup.Remove( hostPath );
			_list.Remove( local );

			string iconPath = GetIconPath( local );
			try
			{
				if( File.Exists( iconPath ) == false )
					File.Delete( iconPath );
			}
			catch
			{
			}
		}

		public GameInformation[] GetGames()
		{
			List<GameInformation> games = new List<GameInformation>( _list.Count );
			foreach( GameInformation game in _list )
			{
				if( game.Ignore == true )
					continue;
				games.Add( game );
			}
			return games.ToArray();
		}

		private string GetIconPath( GameInformation game )
		{
			string iconsPath = Path.Combine( _cachePath, "Icons" );
			try
			{
				if( Directory.Exists( iconsPath ) == false )
					Directory.CreateDirectory( iconsPath );
			}
			catch
			{
			}
			return Path.Combine( iconsPath, game.Parameters.DiscID ) + ".png";
		}

		public Bitmap GetImage( GameInformation game )
		{
			if( game.GameType == GameType.Eboot )
				return BuildGameImage( game );
			else
			{
				string iconPath = GetIconPath( game );
				if( File.Exists( iconPath ) == true )
					return ( Bitmap )Bitmap.FromFile( iconPath );
				else
				{
					Bitmap b = BuildGameImage( game );
					b.Save( iconPath, ImageFormat.Png );
					return b;
				}
			}
		}

		private static Dictionary<string, Bitmap> _regionImages;
		private static void LoadRegionImages()
		{
			_regionImages = new Dictionary<string, Bitmap>();
			_regionImages.Add( "?", Resources.Flag0 );
			_regionImages.Add( "U", Resources.Flag1 );
			//_regionImages.Add( "", Resources.Flag2 ); // EU
			_regionImages.Add( "J", Resources.Flag3 );
			_regionImages.Add( "E", Resources.Flag4 );
			_regionImages.Add( "K", Resources.Flag5 );
			//_regionImages.Add( "A", Resources.Flag6 );
		}

		public const int IconWidth = 90;
		public const int IconHeight = 50;

		public static Bitmap BuildGameImage( GameInformation game )
		{
			Bitmap gameImage;
			if( game.Icon != null )
				gameImage = ( Bitmap )Bitmap.FromStream( game.Icon );
			else
				gameImage = ( Bitmap )Bitmap.FromStream( new MemoryStream( Resources.InvalidIcon, false ) );

			//Image mediaImage;
			//switch( game.GameType )
			//{
			//    default:
			//    case GameType.Eboot:
			//        mediaImage = Resources.SmallMemoryStickIcon;
			//        break;
			//    case GameType.UmdGame:
			//        mediaImage = Resources.SmallUmdIcon;
			//        break;
			//}

			Bitmap regionImage = null;
			if( game.GameType == GameType.UmdGame )
			{
				string regionChar = game.Parameters.DiscID.Substring( 2, 1 );
				if( _regionImages.ContainsKey( regionChar ) == true )
					regionImage = _regionImages[ regionChar ];
				else
					regionImage = _regionImages[ "?" ];
			}

			Bitmap combined = new Bitmap( IconWidth, IconHeight );
			using( Graphics g = Graphics.FromImage( combined ) )
			{
				g.DrawImage( gameImage, 0, 0, combined.Width, combined.Height );
				//g.DrawImage( mediaImage, combined.Width - mediaImage.Width, combined.Height - mediaImage.Height );
				if( regionImage != null )
					g.DrawImageUnscaled( regionImage, combined.Width - regionImage.Width - 2, combined.Height - regionImage.Height - 2 );
			}

			gameImage.Dispose();

			return combined;
		}
	}
}
