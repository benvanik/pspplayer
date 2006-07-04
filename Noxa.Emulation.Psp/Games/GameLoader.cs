using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.IO.Media;
using System.IO;
using System.Drawing;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.IO;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Games
{
	public class GameLoader
	{
		public bool LoadBoot( GameInformation game, IEmulationInstance instance, out uint lowerBounds, out uint upperBounds, out uint entryAddress )
		{
			lowerBounds = 0;
			upperBounds = 0;
			entryAddress = 0;

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
				// Probably in PBP
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
			else
			{
				bootStream = bootBin.OpenRead();
			}

			if( bootStream == null )
			{
				// Whoa!
				Debug.WriteLine( "LoadBoot: game had no boot.bin/eboot.bin, aborting" );
				return false;
			}

			ElfFile elf;
			// TODO: enable exception handling to gracefully handle bad elfs
			//try
			//{
				elf = new ElfFile( bootStream );
			//}
			//catch
			//{
			//	Debug.WriteLine( "LoadBoot: elf load failed, possibly encrypted, aborting" );
			//	return false;
			//}

			uint baseAddress = 0x08900000;

			elf.Load( bootStream, instance, baseAddress );

			// TODO: Move this elsewhere?
			instance.Cpu[ 0 ].ProgramCounter = ( int )elf.InitAddress;
			instance.Cpu[ 0 ].GeneralRegisters[ 28 ] = ( int )elf.GlobalPointer;
			instance.Cpu[ 0 ].GeneralRegisters[ 31 ] = ( int )elf.EntryAddress;

			entryAddress = elf.EntryAddress;
			lowerBounds = baseAddress;
			upperBounds = elf.UpperAddress;

			return true;
		}

		public GameInformation[] FindGames( IEmulationInstance instance )
		{
			List<GameInformation> infos = new List<GameInformation>();

			foreach( IIODriver driver in instance.IO )
			{
				IMediaDevice device = driver as IMediaDevice;
				if( device == null )
					continue;

				GameInformation info;

				switch( device.MediaType )
				{
					case MediaType.MemoryStick:
						// Eboots in PSP\GAME\*
						IMediaFolder rootFolder = device.Root.FindFolder( @"PSP\GAME\" );
						foreach( IMediaFolder folder in rootFolder )
						{
							info = this.GetEbootGameInformation( folder );
							if( info != null )
								infos.Add( info );
						}
						break;
					case MediaType.Umd:
						info = this.GetUmdGameInformation( device );
						if( infos != null )
							infos.Add( info );
						break;
				}
			}

			return infos.ToArray();
		}

		#region Information

		private GameInformation GetEbootGameInformation( IMediaFolder folder )
		{
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
				if( ( gameParams.Category != SfoReader.MemoryStickGameCategory ) &&
					( gameParams.Category != SfoReader.UmdGameCategory ) )
					return null;

				Stream icon = null;
				Stream background = null;
				if( reader.ContainsEntry( PbpReader.PbpEntryType.Icon0 ) == true )
					icon = reader.Read( stream, PbpReader.PbpEntryType.Icon0 );
				if( reader.ContainsEntry( PbpReader.PbpEntryType.Pic1 ) == true )
					background = reader.Read( stream, PbpReader.PbpEntryType.Pic1 );

				return new GameInformation( GameType.Eboot, folder, gameParams, icon, background, null );
			}
		}

		private GameInformation GetUmdGameInformation( IMediaDevice device )
		{
			IMediaFolder folder = device.Root;
			IMediaFile umdData = folder[ "UMD_DATA.BIN" ] as IMediaFile;
			
			//[4 alpha country code]-[4 digit game id]|16 digit binhex|0001|G
			// Get code from SFO
			string uniqueId;
			using( StreamReader umdDataReader = new StreamReader( umdData.OpenRead() ) )
			{
				string line = umdDataReader.ReadToEnd().Trim();
				string[] ps = line.Split( '|' );
				uniqueId = ps[ 1 ];
			}

			IMediaFile sfoData = folder.FindFile( @"PSP_GAME\PARAM.SFO" );
			
			GameParameters gameParams;
			using( Stream stream = sfoData.OpenRead() )
				gameParams = ReadSfo( stream );

			// Only accept games
			if( ( gameParams.Category != SfoReader.MemoryStickGameCategory ) &&
				( gameParams.Category != SfoReader.UmdGameCategory ) )
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
				gp.Category = reader[ "CATEGORY" ].Data as string;
			if( reader[ "DISC_ID" ] != null )
				gp.DiscID = reader[ "DISC_ID" ].Data as string;
			if( reader[ "DISC_VERSION" ] != null )
				gp.GameVersion = new Version( reader[ "DISC_VERSION" ].Data as string );
			if( reader[ "PSP_SYSTEM_VER" ] != null )
				gp.SystemVersion = new Version( reader[ "PSP_SYSTEM_VER" ].Data as string );
			if( reader[ "REGION" ] != null )
				gp.Region = ( int )reader[ "REGION" ].Data;
			
			return gp;
		}

		#endregion
	}
}
