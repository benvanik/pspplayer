// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Games
{
	public enum GameType
	{
		Eboot,
		UmdGame
	}

	public enum GameCategory
	{
		WlanGame,			// WG
		SaveGame,			// MS
		MemoryStickGame,	// MG
		UmdGame,			// UG
		UmdVideo,			// UV
		UmdAudio,			// UA
		CleaningDisc		// UC
	}
	
	public class GameParameters
	{
		public GameCategory Category = GameCategory.MemoryStickGame;
		public int Region = -1;
		public string Title = "Unknown";
		public string DiscID = null;
		public Version GameVersion = new Version();
		public Version SystemVersion = new Version();
		public string Language = null;
	}

	public class GameInformation
	{
		protected GameType _gameType;
		protected IMediaFolder _folder;
		protected string _uniqueId;
		protected GameParameters _parameters;
		protected Stream _icon;
		protected Stream _background;
		protected Stream _dataPsp;
		protected object _tag;

		internal GameInformation( GameType gameType, IMediaFolder folder, GameParameters parameters, Stream icon, Stream background, string uniqueId )
		{
			_gameType = gameType;
			_folder = folder;
			_parameters = parameters;
			_icon = icon;
			_background = background;
			_uniqueId = uniqueId;
		}

		public GameType GameType
		{
			get
			{
				return _gameType;
			}
		}

		public IMediaFolder Folder
		{
			get
			{
				return _folder;
			}
			set
			{
				_folder = value;
			}
		}

		public string UniqueID
		{
			get
			{
				return _uniqueId;
			}
		}

		public GameParameters Parameters
		{
			get
			{
				return _parameters;
			}
		}

		public Stream Icon
		{
			get
			{
				return _icon;
			}
		}

		public Stream Background
		{
			get
			{
				return _background;
			}
		}

		public Stream DataPsp
		{
			get
			{
				return _dataPsp;
			}
			internal set
			{
				_dataPsp = value;
			}
		}

		public object Tag
		{
			get
			{
				return _tag;
			}
			set
			{
				_tag = value;
			}
		}
	}
}
