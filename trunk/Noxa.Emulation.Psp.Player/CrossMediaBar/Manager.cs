// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Noxa.Emulation.Psp.Utilities;
using System.Threading;
using System.IO;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Player.CrossMediaBar
{
	class Manager
	{
		protected Instance _emulator;
		protected VideoManager _video;
		protected SoundManager _sound;
		protected InputManager _input;
		protected Scene _scene;
		protected PerformanceTimer _timer;
		protected bool _isEnabled;
		protected bool _firstRender;

		protected VideoResource _invalidIcon;
		protected VideoResource _iconBackground;

		protected double _lastFrame;

		public Manager( Instance emulator, IntPtr windowHandle, IntPtr controlHandle )
		{
			Debug.Assert( emulator != null );
			Debug.Assert( windowHandle != IntPtr.Zero );
			Debug.Assert( controlHandle != IntPtr.Zero );

			_emulator = emulator;

			_video = new VideoManager( this, controlHandle );
			_sound = new SoundManager();
			_input = new InputManager( _emulator, windowHandle );

			byte[] bytes = Properties.Resources.InvalidIcon;
			_invalidIcon = _video.CreateImageResource( new MemoryStream( bytes ) );
			bytes = Properties.Resources.IconBackground;
			_iconBackground = _video.CreateImageResource( new MemoryStream( bytes ) );

			_scene = new Scene( this );

			_timer = new PerformanceTimer();
			_lastFrame = _timer.Elapsed;
		}

		public bool IsEnabled
		{
			get
			{
				return _isEnabled;
			}
		}

		public VideoManager Video
		{
			get
			{
				return _video;
			}
		}

		public SoundManager Sound
		{
			get
			{
				return _sound;
			}
		}

		public Instance Emulator
		{
			get
			{
				return _emulator;
			}
		}

		internal bool FirstRender
		{
			get
			{
				return _firstRender;
			}
			set
			{
				_firstRender = value;
			}
		}

		internal VideoResource InvalidIcon
		{
			get
			{
				return _invalidIcon;
			}
		}

		internal VideoResource IconBackground
		{
			get
			{
				return _iconBackground;
			}
		}

		public void Enable()
		{
			if( _isEnabled == true )
				return;

			_isEnabled = true;

			_video.Enable();
			_sound.Enable();

			if( _video.IsValid == false )
			{
				// Unable to create video, so can't use us!
				// TODO: fall back to a winforms UI here
				return;
			}
		}

		public void Disable()
		{
			if( _isEnabled == false )
				return;

			_isEnabled = false;

			_sound.Disable();
			_video.Disable();
		}

		public void Update()
		{
			double thisFrame = _timer.Elapsed;
			double elapsed = thisFrame - _lastFrame;
			_lastFrame = thisFrame;

			_input.Update();
			
			_video.Begin();
			_scene.Update( elapsed, _input.Events );
			if( _firstRender == true )
			{
				_input.Update();
				_scene.Update( elapsed, _input.Events );
				_firstRender = false;
			}
			_video.End();

			_sound.Update();
		}

		public void LaunchGame( GameInformation game )
		{
			_emulator.SwitchToGame( game );
		}

		public void Cleanup()
		{
		}
	}
}
