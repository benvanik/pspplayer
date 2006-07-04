using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.IO.Media;
using System.Drawing;
using System.IO;

namespace Noxa.Emulation.Psp.Games
{
	public enum GameType
	{
		Eboot,
		UmdGame
	}

	public class GameParameters
	{
		public string Category = "MG"; // No clue what this is ^_^
		public int Region = 0;
		public string Title = "Unknown";
		public string DiscID = null;
		public Version GameVersion = new Version();
		public Version SystemVersion = new Version();
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
	}
}
