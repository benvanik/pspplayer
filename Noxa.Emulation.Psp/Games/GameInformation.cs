// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Games
{
	/// <summary>
	/// Describes the type of game.
	/// </summary>
	public enum GameType
	{
		/// <summary>
		/// Eboot as loaded off of a Memory Stick.
		/// </summary>
		Eboot,

		/// <summary>
		/// Retail UMD game.
		/// </summary>
		UmdGame
	}

	/// <summary>
	/// Describes the game category.
	/// </summary>
	public enum GameCategory
	{
		/// <summary>
		/// A gameshare game (WG).
		/// </summary>
		WlanGame,
		/// <summary>
		/// Save game data (MS).
		/// </summary>
		SaveGame,
		/// <summary>
		/// A Memory Stick game in the form of an EBOOT (MG).
		/// </summary>
		MemoryStickGame,
		/// <summary>
		/// A retail UMD game (UG).
		/// </summary>
		UmdGame,
		/// <summary>
		/// A UMD video title (UV).
		/// </summary>
		UmdVideo,
		/// <summary>
		/// A UMD audio title (UA).
		/// </summary>
		UmdAudio,
		/// <summary>
		/// A UMD cleaning disc (UC).
		/// </summary>
		CleaningDisc,
	}
	
	/// <summary>
	/// Game parameters.
	/// </summary>
	public class GameParameters
	{
		/// <summary>
		/// The category the game falls in to.
		/// </summary>
		public GameCategory Category = GameCategory.MemoryStickGame;

		/// <summary>
		/// The region the game is assigned to.
		/// </summary>
		public int Region = -1;

		/// <summary>
		/// The title of the game.
		/// </summary>
		public string Title = "Unknown";

		/// <summary>
		/// The ID of the game title.
		/// </summary>
		public string DiscID = null;

		/// <summary>
		/// The version of the game.
		/// </summary>
		public Version GameVersion = new Version();

		/// <summary>
		/// The system version required by the game.
		/// </summary>
		public Version SystemVersion = new Version();

		/// <summary>
		/// The language of the game.
		/// </summary>
		public string Language = null;
	}

	/// <summary>
	/// Game information.
	/// </summary>
	public class GameInformation
	{
		/// <summary>
		/// The game type.
		/// </summary>
		public readonly GameType GameType;

		/// <summary>
		/// The unique ID of the UMD.
		/// </summary>
		public readonly string UniqueID;

		/// <summary>
		/// Game parameters.
		/// </summary>
		public readonly GameParameters Parameters;

		/// <summary>
		/// Game icon image.
		/// </summary>
		public readonly Stream Icon;

		/// <summary>
		/// Game background image.
		/// </summary>
		public readonly Stream Background;

		/// <summary>
		/// Folder where the game is located.
		/// </summary>
		public IMediaFolder Folder;

		/// <summary>
		/// The DATA.PSP entry.
		/// </summary>
		public Stream DataPsp;

		/// <summary>
		/// User-defined tag.
		/// </summary>
		public object Tag;

		internal GameInformation( GameType gameType, IMediaFolder folder, GameParameters parameters, Stream icon, Stream background, string uniqueId )
		{
			this.GameType = gameType;
			this.UniqueID = uniqueId;
			this.Parameters = parameters;
			this.Icon = icon;
			this.Background = background;

			this.Folder = folder;
		}
	}
}
