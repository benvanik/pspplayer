// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.IO;
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Games
{
	/// <summary>
	/// Utility class for finding and loading games.
	/// </summary>
	public class GameLoader
	{
		#region Load Helpers

		/// <summary>
		/// Find and retrieve the boot stream for the given game.
		/// </summary>
		/// <param name="game">Game to look for.</param>
		/// <returns>The games boot stream (from BOOT.BIN, etc) or <c>null</c> if it could not be found.</returns>
		public Stream FindBootStream( GameInformation game )
		{
			Debug.Assert( game != null );
			if( game == null )
				return null;

			IMediaFolder folder = game.Folder;
			if( folder[ "PSP_GAME" ] != null )
				folder = folder[ "PSP_GAME" ] as IMediaFolder;
			if( folder[ "SYSDIR" ] != null )
				folder = folder[ "SYSDIR" ] as IMediaFolder;
			IMediaFile bootBin = null;
			bootBin = folder[ "BOOT.BIN" ] as IMediaFile;
			if( bootBin == null )
				bootBin = folder[ "EBOOT.BIN" ] as IMediaFile;
			if( bootBin == null )
				bootBin = folder[ "BOOT.ELF" ] as IMediaFile;
			if( bootBin == null )
				bootBin = folder[ "EBOOT.ELF" ] as IMediaFile;

			Stream bootStream = null;
			if( bootBin == null )
			{
				// Probably in PBP - unless exploited!
				if( folder.Name.Contains( "__SCE__" ) == true )
				{
					// If this is exploited, the eboot.pbp IS the elf!
					bootStream = ( folder[ "EBOOT.PBP" ] as IMediaFile ).OpenRead();
				}
				else
				{
					IMediaFile pbp = folder[ "EBOOT.PBP" ] as IMediaFile;
					using( Stream stream = pbp.OpenRead() )
					{
						PbpReader reader = new PbpReader( stream );
						if( reader.ContainsEntry( PbpReader.PbpEntryType.DataPsp ) == true )
						{
							bootStream = reader.Read( stream, PbpReader.PbpEntryType.DataPsp );
						}
					}
				}
			}
			else
			{
				bootStream = bootBin.OpenRead();
			}

			return bootStream;
		}

		/// <summary>
		/// Generate a load report XML file and write it to the disk.
		/// </summary>
		/// <param name="instance">The current emulation instance.</param>
		/// <param name="game">The game being loaded.</param>
		/// <param name="results">The results of the load.</param>
		public void GenerateReport( IEmulationInstance instance, GameInformation game, LoadResults results )
		{
			Debug.Assert( game != null );
			if( game == null )
				return;
			Debug.Assert( results != null );
			if( results == null )
				return;

			XmlDocument doc = new XmlDocument();
			doc.AppendChild( doc.CreateXmlDeclaration( "1.0", null, "yes" ) );
			XmlElement root = doc.CreateElement( "loadResults" );
			root.SetAttribute( "successful", results.Successful.ToString() );

			XmlElement gameRoot = doc.CreateElement( "game" );
			gameRoot.SetAttribute( "type", game.GameType.ToString() );
			//gameRoot.SetAttribute( "path", EscapeFilePath( game.Folder.AbsolutePath ) );
			{
				XmlElement sfoRoot = doc.CreateElement( "parameters" );
				if( game.Parameters.Title != null )
					sfoRoot.SetAttribute( "title", game.Parameters.Title );
				if( game.Parameters.SystemVersion != new Version() )
					sfoRoot.SetAttribute( "systemVersion", game.Parameters.SystemVersion.ToString() );
				if( game.Parameters.GameVersion != new Version() )
					sfoRoot.SetAttribute( "gameVersion", game.Parameters.GameVersion.ToString() );
				if( game.Parameters.DiscID != null )
					sfoRoot.SetAttribute( "discId", game.Parameters.DiscID );
				if( game.Parameters.Region >= 0 )
					sfoRoot.SetAttribute( "region", game.Parameters.Region.ToString( "X" ) );
				if( game.Parameters.Language != null )
					sfoRoot.SetAttribute( "language", game.Parameters.Language );
				sfoRoot.SetAttribute( "category", game.Parameters.Category.ToString() );
				gameRoot.AppendChild( sfoRoot );
			}
			root.AppendChild( gameRoot );

			XmlElement biosRoot = doc.CreateElement( "bios" );
			{
				IComponent factory = Activator.CreateInstance( instance.Bios.Factory ) as IComponent;
				if( factory != null )
				{
					biosRoot.SetAttribute( "name", factory.Name );
					biosRoot.SetAttribute( "version", factory.Version.ToString() );
					biosRoot.SetAttribute( "build", factory.Build.ToString() );
					if( factory.Author != null )
						biosRoot.SetAttribute( "author", factory.Author );
					if( factory.Website != null )
						biosRoot.SetAttribute( "website", factory.Website );
				}
			}
			root.AppendChild( biosRoot );

			XmlElement importsRoot = doc.CreateElement( "imports" );
			foreach( StubImport import in results.Imports )
			{
				XmlElement importRoot = doc.CreateElement( "import" );
				importRoot.SetAttribute( "type", import.Result.ToString() );
				importRoot.SetAttribute( "module", import.ModuleName );
				importRoot.SetAttribute( "nid", string.Format( "{0:X8}", import.NID ) );

				if( import.Function != null )
				{
					XmlElement functionRoot = doc.CreateElement( "function" );
					functionRoot.SetAttribute( "name", import.Function.Name );
					functionRoot.SetAttribute( "isImplemented", import.Function.IsImplemented.ToString() );
					functionRoot.SetAttribute( "hasNative", ( import.Function.NativeMethod != IntPtr.Zero ) ? true.ToString() : false.ToString() );
					importRoot.AppendChild( functionRoot );
				}

				importsRoot.AppendChild( importRoot );
			}
			root.AppendChild( importsRoot );

			doc.AppendChild( root );

			string fileName;
			if( game.GameType == GameType.Eboot )
			{
				string title = game.Parameters.Title;
				title = title.Replace( "\n", "" ); // All it takes is one moron
				fileName = string.Format( "LoadResult-Eboot-{0}.xml", title );
			}
			else
				fileName = string.Format( "LoadResult-{0}.xml", game.Parameters.DiscID );
			using( FileStream stream = File.Open( fileName, FileMode.Create ) )
				doc.Save( stream );
		}

		private static string EscapeFilePath( string path )
		{
			return path.Replace( "&", "&amp;" ).Replace( "'", "&apos;" );
		}

		#endregion

		#region Game Searching

		/// <summary>
		/// Find all games on the current Memory Stick.
		/// </summary>
		/// <param name="device">The Memory Stick to search.</param>
		/// <returns>A list of games found.</returns>
		public GameInformation[] FindGames( IMemoryStickDevice device )
		{
			List<GameInformation> infos = new List<GameInformation>();

			Debug.Assert( device != null );
			if( ( device != null ) &&
				( device.State == MediaState.Present ) )
			{
				// Eboots in PSP\GAME\*
				IMediaFolder rootFolder = device.Root.FindFolder( @"PSP\GAME\" );
				foreach( IMediaFolder folder in rootFolder )
				{
					// kxploit check
					IMediaFolder realFolder = folder;
					bool wasExploited = false;
					if( folder.Name.StartsWith( "__SCE__" ) == true )
					{
						// Find the %__SCE__... folder
						realFolder = folder.Parent[ "%" + folder.Name ] as IMediaFolder;
						Debug.Assert( realFolder != null );
						wasExploited = true;
					}
					else if( folder.Name[ 0 ] == '%' )
						continue;
					GameInformation info = this.GetEbootGameInformation( realFolder );
					if( wasExploited == true )
						info.Folder = folder;
					if( info != null )
						infos.Add( info );
				}
			}

			return infos.ToArray();
		}

		/// <summary>
		/// Find the game on the given UMD device.
		/// </summary>
		/// <param name="device">The UMD to search.</param>
		/// <returns>The game found on the device, or <c>null</c> if none was found.</returns>
		public GameInformation FindGame( IUmdDevice device )
		{
			Debug.Assert( device != null );
			if( device == null )
				return null;

			GameInformation info = this.GetUmdGameInformation( device );
			Debug.Assert( info != null );

			return info;
		}

		#endregion

		#region Information

		/// <summary>
		/// Get <see cref="GameInformation"/> from the given EBOOT folder.
		/// </summary>
		/// <param name="folder">The folder containing the EBOOT.</param>
		/// <returns>A <see cref="GameInformation"/> instance representing the game, or <c>null</c> if an error occurred.</returns>
		public GameInformation GetEbootGameInformation( IMediaFolder folder )
		{
			Debug.Assert( folder != null );
			if( folder == null )
				return null;

			IMediaFile file = folder[ "EBOOT.PBP" ] as IMediaFile;
			if( file == null )
				return null;

			using( Stream stream = file.OpenRead() )
			{
				PbpReader reader = new PbpReader( stream );

				GameParameters gameParams;
				using( Stream pdbStream = reader.Read( stream, PbpReader.PbpEntryType.Param ) )
					gameParams = ReadSfo( pdbStream );

				// Only accept games
				if( ( gameParams.Category != GameCategory.MemoryStickGame ) &&
					( gameParams.Category != GameCategory.UmdGame ) )
					return null;

				Stream icon = null;
				Stream background = null;
				if( reader.ContainsEntry( PbpReader.PbpEntryType.Icon0 ) == true )
					icon = reader.Read( stream, PbpReader.PbpEntryType.Icon0 );
				//if( reader.ContainsEntry( PbpReader.PbpEntryType.Pic1 ) == true )
				//	background = reader.Read( stream, PbpReader.PbpEntryType.Pic1 );

				return new GameInformation( GameType.Eboot, folder, gameParams, icon, background, null );
			}
		}

		/// <summary>
		/// Get <see cref="GameInformation"/> from the given UMD device.
		/// </summary>
		/// <param name="device">The device containing the game.</param>
		/// <returns>A <see cref="GameInformation"/> instance representing the game, or <c>null</c> if an error occurred.</returns>
		public GameInformation GetUmdGameInformation( IMediaDevice device )
		{
			IMediaFolder folder = device.Root;
			IMediaFile umdData = folder[ "UMD_DATA.BIN" ] as IMediaFile;
			
			//[4 alpha country code]-[4 digit game id]|16 digit binhex|0001|G
			// Get code from SFO
			string uniqueId;
			using( StreamReader reader = new StreamReader( umdData.OpenRead() ) )
			{
				string line = reader.ReadToEnd().Trim();
				string[] ps = line.Split( '|' );
				uniqueId = ps[ 1 ];
			}

			IMediaFile sfoData = folder.FindFile( @"PSP_GAME\PARAM.SFO" );
			
			GameParameters gameParams;
			using( Stream stream = sfoData.OpenRead() )
				gameParams = ReadSfo( stream );

			// Only accept games
			if( ( gameParams.Category != GameCategory.MemoryStickGame ) &&
				( gameParams.Category != GameCategory.UmdGame ) )
				return null;

			Stream icon = null;
			Stream background = null;
			IMediaFile iconData = folder.FindFile( @"PSP_GAME\ICON0.PNG" );
			if( iconData != null )
				icon = iconData.OpenRead();
			IMediaFile bgData = folder.FindFile( @"PSP_GAME\PIC1.PNG" );
			if( bgData != null )
				background = bgData.OpenRead();

			return new GameInformation( GameType.UmdGame, folder, gameParams, icon, background, uniqueId );
		}

		private GameParameters ReadSfo( Stream sfo )
		{
			GameParameters gp = new GameParameters();
			SfoReader reader = new SfoReader( sfo );

			if( reader[ "TITLE" ] != null )
				gp.Title = reader[ "TITLE" ].Data as string;
			if( reader[ "CATEGORY" ] != null )
			{
				string categoryString = reader[ "CATEGORY" ].Data as string;
				switch( categoryString )
				{
					case "WG":
						gp.Category = GameCategory.WlanGame;
						break;
					case "MS":
						gp.Category = GameCategory.SaveGame;
						break;
					case "UG":
						gp.Category = GameCategory.UmdGame;
						break;
					case "UV":
						gp.Category = GameCategory.UmdVideo;
						break;
					case "UA":
						gp.Category = GameCategory.UmdAudio;
						break;
					case "UC":
						gp.Category = GameCategory.CleaningDisc;
						break;
					case "MG":
					default:
						gp.Category = GameCategory.MemoryStickGame;
						break;
				}
			}
			if( reader[ "DISC_ID" ] != null )
				gp.DiscID = reader[ "DISC_ID" ].Data as string;
			if( reader[ "DISC_VERSION" ] != null )
				gp.GameVersion = new Version( reader[ "DISC_VERSION" ].Data as string );
			if( reader[ "PSP_SYSTEM_VER" ] != null )
				gp.SystemVersion = new Version( reader[ "PSP_SYSTEM_VER" ].Data as string );
			if( reader[ "REGION" ] != null )
				gp.Region = ( int )reader[ "REGION" ].Data;
			if( reader[ "LANGUAGE" ] != null )
				gp.Language = reader[ "LANGUAGE" ].Data as string;
			
			return gp;
		}

		#endregion
	}
}
