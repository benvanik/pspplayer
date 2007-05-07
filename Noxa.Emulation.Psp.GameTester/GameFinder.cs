// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Noxa.Emulation.Psp.Games;
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.GameTester
{
	static class GameFinder
	{
		public static List<GameInformation> FindGames( string path )
		{
			List<GameInformation> games = new List<GameInformation>();

			foreach( string umdFile in Directory.GetFiles( path, "*.iso" ) )
			{
				string umdPath = Path.Combine( path, umdFile );
				IUmdDevice device = ( new Noxa.Emulation.Psp.Media.Iso.IsoFileSystem() ).CreateInstance( null, new ComponentParameters() ) as IUmdDevice;
				device.Load( umdPath, true );
				GameInformation info = GameLoader.FindGame( device );
				games.Add( info );
			}

			return games;
		}
	}
}
