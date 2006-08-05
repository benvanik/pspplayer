using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Games;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Noxa.Emulation.Psp.IO.Media;
using Noxa.Emulation.Psp.IO;
using Noxa.Emulation.Psp.IO.Input;

namespace Noxa.Emulation.Psp.Player.CrossMediaBar
{
	class Scene
	{
		protected Manager _manager;
		protected List<BarGroup> _groups;
		protected BarGroup _currentGroup;
		protected BarItem _currentItem;
		protected VideoResource _background;
		protected VideoResource _bootVideo;
		protected bool _playVideo;

		protected Dictionary<PadButtons, double> _repeats = new Dictionary<PadButtons, double>();

		protected bool _booting;
		protected double _bootTime;
		protected BarItem _bootTarget;
		protected const double BootDuration = 3.0;

		protected const double RepeatInterval = 0.15;

		public Scene( Manager manager )
		{
			Debug.Assert( manager != null );

			_manager = manager;

			byte[] bytes;
			switch( DateTime.Now.Month )
			{
				default:
				case 1:
					bytes = Properties.Resources.Wallpaper01;
					break;
				case 2:
					bytes = Properties.Resources.Wallpaper02;
					break;
				case 3:
					bytes = Properties.Resources.Wallpaper03;
					break;
				case 4:
					bytes = Properties.Resources.Wallpaper04;
					break;
				case 5:
					bytes = Properties.Resources.Wallpaper05;
					break;
				case 6:
					bytes = Properties.Resources.Wallpaper06;
					break;
				case 7:
					bytes = Properties.Resources.Wallpaper07;
					break;
				case 8:
					bytes = Properties.Resources.Wallpaper08;
					break;
				case 9:
					bytes = Properties.Resources.Wallpaper09;
					break;
				case 10:
					bytes = Properties.Resources.Wallpaper10;
					break;
				case 11:
					bytes = Properties.Resources.Wallpaper11;
					break;
				case 12:
					bytes = Properties.Resources.Wallpaper12;
					break;
			}
			_background = _manager.Video.CreateImageResource( new MemoryStream( bytes, 0, bytes.Length, false, false ) );

#if USEVIDEO
			bytes = Properties.Resources.PspBoot;
			_bootVideo = _manager.Video.CreateVideoResource( bytes );
			_bootVideo.Width = 480;
			_bootVideo.Height = 272;
#endif

			_repeats.Add( PadButtons.Circle, 0.0 );
			_repeats.Add( PadButtons.Cross, 0.0 );
			_repeats.Add( PadButtons.DigitalDown, 0.0 );
			_repeats.Add( PadButtons.DigitalLeft, 0.0 );
			_repeats.Add( PadButtons.DigitalRight, 0.0 );
			_repeats.Add( PadButtons.DigitalUp, 0.0 );
			_repeats.Add( PadButtons.Hold, 0.0 );
			_repeats.Add( PadButtons.Home, 0.0 );
			_repeats.Add( PadButtons.LeftTrigger, 0.0 );
			_repeats.Add( PadButtons.MusicNote, 0.0 );
			_repeats.Add( PadButtons.RightTrigger, 0.0 );
			_repeats.Add( PadButtons.Select, 0.0 );
			_repeats.Add( PadButtons.Square, 0.0 );
			_repeats.Add( PadButtons.Start, 0.0 );
			_repeats.Add( PadButtons.Triangle, 0.0 );

			this.Refresh();
		}

		private BarGroup BuildUmdGroup( GameInformation[] games )
		{
			foreach( GameInformation game in games )
			{
				if( game.GameType == GameType.UmdGame )
				{
					BarGroup group = new UmdBarGroup( game );
					return group;
				}
			}

			return null;
		}

		private BarGroup BuildMemoryStickGroup( GameInformation[] games )
		{
			List<GameInformation> msGames = new List<GameInformation>();
			foreach( GameInformation game in games )
			{
				if( game.GameType == GameType.Eboot )
					msGames.Add( game );
			}

			return new MemoryStickBarGroup( msGames );
		}

		public void Refresh()
		{
			GameLoader loader = new GameLoader();
			GameInformation[] games = loader.FindGames( _manager.Emulator );

			BarGroup newGroup;
			_groups = new List<BarGroup>();
			newGroup = this.BuildMemoryStickGroup( games );
			if( newGroup != null )
				_groups.Add( newGroup );
			newGroup = this.BuildUmdGroup( games );
			if( newGroup != null )
				_groups.Add( newGroup );

			foreach( BarGroup group in _groups )
			{
				group.Sort();
				group.Create( _manager );
			}

			if( _currentGroup != null )
			{
				bool foundGroup = false;
				foreach( BarGroup group in _groups )
				{
					if( group.Name == _currentGroup.Name )
					{
						_currentGroup = group;
						foundGroup = true;
						break;
					}
				}
				if( foundGroup == false )
				{
					_currentGroup = null;
					_currentItem = null;
				}
				else
				{
					bool foundItem = false;
					foreach( BarItem item in _currentGroup.Items )
					{
						if( item.Name == _currentItem.Name )
						{
							_currentItem = item;
							foundItem = true;
							break;
						}
					}
					if( foundItem == false )
						_currentItem = null;
				}
			}

			if( _currentGroup == null )
			{
				foreach( BarGroup group in _groups )
				{
					if( group.Items.Count > 0 )
					{
						_currentGroup = group;
						break;
					}
				}
				if( _currentGroup == null )
					_currentGroup = _groups[ 0 ];
			}
			if( ( _currentItem == null ) &&
				( _currentGroup != null ) &&
				( _currentGroup.Items.Count > 0 ) )
			{
				_currentItem = _currentGroup.Items[ 0 ];
			}
		}

		public void Update( double elapsed, IList<InputEvent> events )
		{
			if( _booting == false )
			{
				int groupMove = 0;
				int itemMove = 0;
				bool itemAction = false;
				bool refreshAction = false;
				for( int n = 0; n < events.Count; n++ )
				{
					InputEvent ev = events[ n ];

					bool repeating = false;
					if( ev.EventType == InputEventType.Released )
					{
						_repeats[ ev.Button ] = 0.0;
					}
					else if( ev.EventType == InputEventType.Unchanged )
					{
						double time = _repeats[ ev.Button ];
						time += elapsed;

						if( time >= RepeatInterval )
						{
							// Only allow repeats on movement keys
							if( ( ev.Button == PadButtons.DigitalUp ) ||
								( ev.Button == PadButtons.DigitalDown ) ||
								( ev.Button == PadButtons.DigitalLeft ) ||
								( ev.Button == PadButtons.DigitalRight ) ||
								( ev.Button == PadButtons.LeftTrigger ) ||
								( ev.Button == PadButtons.RightTrigger ) )
							{
								repeating = true;
							}
							time = 0.0;
						}

						_repeats[ ev.Button ] = time;
					}

					if( ( repeating == false ) &&
						( ev.EventType != InputEventType.Depressed ) )
						continue;

					switch( ev.Button )
					{
						case Noxa.Emulation.Psp.IO.Input.PadButtons.DigitalUp:
							itemMove = -1;
							break;
						case Noxa.Emulation.Psp.IO.Input.PadButtons.DigitalDown:
							itemMove = 1;
							break;
						case Noxa.Emulation.Psp.IO.Input.PadButtons.LeftTrigger:
							itemMove = -5;
							break;
						case Noxa.Emulation.Psp.IO.Input.PadButtons.RightTrigger:
							itemMove = 5;
							break;

						case Noxa.Emulation.Psp.IO.Input.PadButtons.DigitalLeft:
							groupMove = -1;
							break;
						case Noxa.Emulation.Psp.IO.Input.PadButtons.DigitalRight:
							groupMove = 1;
							break;

						case Noxa.Emulation.Psp.IO.Input.PadButtons.Cross:
							itemAction = true;
							break;
						case Noxa.Emulation.Psp.IO.Input.PadButtons.Circle:
							itemAction = true;
							break;

						case Noxa.Emulation.Psp.IO.Input.PadButtons.Select:
							refreshAction = true;
							break;
						case Noxa.Emulation.Psp.IO.Input.PadButtons.Start:
							refreshAction = true;
							break;
					}
				}

				Move( groupMove, itemMove );

				if( itemAction == true )
				{
					_bootTarget = _currentItem;

					if( _playVideo == true )
					{
						_bootTime = 0.0;
						_booting = true;

#if USEVIDEO
						( _bootVideo as Noxa.Emulation.Psp.Player.CrossMediaBar.VideoManager.VideoRenderer ).Play();
#endif
					}
					else
					{
						this.ForceLaunch();
					}

					return;
				}
				else if( refreshAction == true )
				{
					Debug.WriteLine( "XMB: Refreshing game listing" );

					foreach( IIODriver driver in _manager.Emulator.IO )
					{
						IMediaDevice device = driver as IMediaDevice;
						if( device == null )
							continue;

						device.Refresh();
					}

					this.Refresh();

					_manager.Video.Recreate();
					_manager.Sound.Recreate();

					return;
				}

				if( ( itemAction == true ) ||
					( refreshAction == true ) )
					_manager.Sound.Play( Sounds.Tick );

				if( ( _currentGroup != null ) &&
					( _currentGroup.BackgroundResource != null ) )
				{
					_currentGroup.BackgroundResource.Draw( 0.0f, 0.0f );
				}
				else if( _background != null )
					_background.Draw( 0.0f, 0.0f );

				float groupX = 8.0f;
				float groupY = 5.0f;

				for( int n = 0; n < _groups.Count; n++ )
				{
					BarGroup group = _groups[ n ];

					SizeF size = group.Draw( groupX, groupY, _currentGroup, _currentItem );

					groupX += size.Width + 8.0f;
				}
			}
			else
			{
				_bootTime += elapsed;
				if( CheckEndVideo() == true )
					return;

#if USEVIDEO
				while( ( _bootVideo as VideoManager.VideoRenderer ).FrameWaiting.WaitOne( 150, false ) == false )
				{
					_bootTime += 0.100;
					if( CheckEndVideo() == true )
						return;
				}

				_bootVideo.Draw( 0.0f, 0.0f );
#endif
			}
		}

		private void ForceLaunch()
		{
			_booting = false;
			_bootTime = 0.0;

			_manager.Video.Clear();

			// Launch boottarget through manager
			GameInformation game = ( _bootTarget as GameBarItem ).Game;
			_manager.LaunchGame( game );
		}

		private bool CheckEndVideo()
		{
			if( _bootTime > BootDuration )
			{
				this.ForceLaunch();

				return true;
			}

			return false;
		}

		private void Move( int groupMove, int itemMove )
		{
			if( _groups.Count == 0 )
				return;
			if( _currentGroup == null )
				return;
			if( _currentGroup.Items.Count == 0 )
				itemMove = 0;

			if( itemMove != 0 )
				groupMove = 0;

			bool moved = false;

			if( itemMove != 0 )
			{
				int newItemIndex = _currentGroup.Items.IndexOf( _currentItem ) + itemMove;
				if( newItemIndex < 0 )
					newItemIndex = 0;
				else if( newItemIndex >= _currentGroup.Items.Count )
					newItemIndex = _currentGroup.Items.Count - 1;

				if( _currentItem != _currentGroup.Items[ newItemIndex ] )
				{
					moved = true;

					_currentItem = _currentGroup.Items[ newItemIndex ];
				}
			}

			if( groupMove != 0 )
			{
				int newGroupIndex = _groups.IndexOf( _currentGroup ) + groupMove;
				if( newGroupIndex < 0 )
					newGroupIndex = 0;
				else if( newGroupIndex >= _groups.Count )
					newGroupIndex = _groups.Count - 1;

				if( _currentGroup != _groups[ newGroupIndex ] )
				{
					moved = true;

					_currentGroup = _groups[ newGroupIndex ];
					if( _currentGroup.Items.Count > 0 )
						_currentItem = _currentGroup.Items[ 0 ];
					else
						_currentItem = null;
				}
			}

			if( moved == true )
				_manager.Sound.Play( Sounds.Tick );
		}

		public void Cleanup()
		{
		}
	}
}
