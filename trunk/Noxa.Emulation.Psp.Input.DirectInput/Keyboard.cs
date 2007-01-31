// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.DirectX.DirectInput;

namespace Noxa.Emulation.Psp.IO.Input.DirectInput
{
	class Keyboard : IInputDevice
	{
		protected IEmulationInstance _emulator;
		protected ComponentParameters _params;
		protected IntPtr _windowHandle;
		protected Device _device;
		
		protected PadButtons _buttons;
		protected int _analogX;
		protected int _analogY;

		public Keyboard( IEmulationInstance emulator, ComponentParameters parameters )
		{
			//Debug.Assert( emulator != null );
			Debug.Assert( parameters != null );

			_emulator = emulator;
			_params = parameters;
		}

		public ComponentParameters Parameters
		{
			get
			{
				return _params;
			}
		}

		public IEmulationInstance Emulator
		{
			get
			{
				return _emulator;
			}
		}

		public Type Factory
		{
			get
			{
				return typeof( DirectInputDriver );
			}
		}

		public event EventHandler Connected;
		public event EventHandler Disconnected;

		public IntPtr WindowHandle
		{
			get
			{
				return _windowHandle;
			}
			set
			{
				_windowHandle = value;
			}
		}

		public bool IsConnected
		{
			get
			{
				return ( _device != null );
			}
		}

		public int AnalogX
		{
			get
			{
				return _analogX;
			}
		}

		public int AnalogY
		{
			get
			{
				return _analogY;
			}
		}

		public PadButtons Buttons
		{
			get
			{
				return _buttons;
			}
		}

		public void Poll()
		{
			if( _device == null )
			{
				Debug.Assert( _windowHandle != IntPtr.Zero );
				if( _windowHandle == IntPtr.Zero )
					return;

				try
				{
					_device = new Device( SystemGuid.Keyboard );
					_device.SetDataFormat( DeviceDataFormat.Keyboard );
					_device.SetCooperativeLevel( _windowHandle, CooperativeLevelFlags.Foreground | CooperativeLevelFlags.NonExclusive );
				}
				catch
				{
					_device.Dispose();
					_device = null;
					return;
				}
			}

			KeyboardState state = null;
			try
			{
				_device.Poll();
				state = _device.GetCurrentKeyboardState();
			}
			catch
			{
				try
				{
					_device.Acquire();
					_device.Poll();
				}
				catch
				{
					_buttons = PadButtons.None;
					_analogX = 0;
					_analogY = 0;
					return;
				}
			}

			// Keys:
			// wsad = dpad
			// l/r shift = l/r shoulder
			// p;l' = tri cross squ cir
			// z = start, x = sel, c = home

			if( state != null )
			{
				PadButtons buttons = PadButtons.None;
				if( ( state[ Key.SemiColon ] == true ) ||
					( state[ Key.Return ] == true ) )
					buttons |= PadButtons.Cross;
				if( state[ Key.Apostrophe ] == true )
					buttons |= PadButtons.Circle;
				if( state[ Key.L ] == true )
					buttons |= PadButtons.Square;
				if( state[ Key.P ] == true )
					buttons |= PadButtons.Triangle;
				if( state[ Key.Z ] == true )
					buttons |= PadButtons.Start;
				if( state[ Key.X ] == true )
					buttons |= PadButtons.Select;
				if( state[ Key.C ] == true )
					buttons |= PadButtons.Home;
				if( ( state[ Key.W ] == true ) ||
					( state[ Key.Up ] == true ) )
					buttons |= PadButtons.DigitalUp;
				if( ( state[ Key.S ] == true ) ||
					( state[ Key.Down ] == true ) )
					buttons |= PadButtons.DigitalDown;
				if( ( state[ Key.A ] == true ) ||
					( state[ Key.Left ] == true ) )
					buttons |= PadButtons.DigitalLeft;
				if( ( state[ Key.D ] == true ) ||
					( state[ Key.Right ] == true ) )
					buttons |= PadButtons.DigitalRight;
				if( state[ Key.LeftShift ] == true )
					buttons |= PadButtons.LeftTrigger;
				if( state[ Key.RightShift ] == true )
					buttons |= PadButtons.RightTrigger;
				_buttons = buttons;
			}
			else
				_buttons = PadButtons.None;

			_analogX = 0;
			_analogY = 0;
		}

		public void Cleanup()
		{
			if( _device != null )
			{
				try
				{
					_device.Unacquire();
				}
				catch
				{
				}
				_device.Dispose();
				_device = null;
			}
		}
	}
}
