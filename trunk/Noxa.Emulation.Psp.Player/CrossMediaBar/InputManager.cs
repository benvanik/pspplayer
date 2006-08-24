// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Noxa.Emulation.Psp.IO.Input;
using Noxa.Emulation.Psp.IO;

namespace Noxa.Emulation.Psp.Player.CrossMediaBar
{
	class InputState
	{
		public int AnalogX;
		public int AnalogY;
		public PadButtons Buttons;

		public bool UpButton
		{
			get
			{
				return ( Buttons & PadButtons.DigitalUp ) == PadButtons.DigitalUp;
			}
		}

		public bool DownButton
		{
			get
			{
				return ( Buttons & PadButtons.DigitalDown ) == PadButtons.DigitalDown;
			}
		}

		public bool LeftButton
		{
			get
			{
				return ( Buttons & PadButtons.DigitalLeft ) == PadButtons.DigitalLeft;
			}
		}

		public bool RightButton
		{
			get
			{
				return ( Buttons & PadButtons.DigitalRight ) == PadButtons.DigitalRight;
			}
		}

		public bool StartButton
		{
			get
			{
				return ( Buttons & PadButtons.Start ) == PadButtons.Start;
			}
		}

		public bool SelectButton
		{
			get
			{
				return ( Buttons & PadButtons.Select ) == PadButtons.Select;
			}
		}

		public bool HomeButton
		{
			get
			{
				return ( Buttons & PadButtons.Home ) == PadButtons.Home;
			}
		}

		public bool LeftTriggerButton
		{
			get
			{
				return ( Buttons & PadButtons.LeftTrigger ) == PadButtons.LeftTrigger;
			}
		}

		public bool RightTriggerButton
		{
			get
			{
				return ( Buttons & PadButtons.RightTrigger ) == PadButtons.RightTrigger;
			}
		}

		public bool CrossButton
		{
			get
			{
				return ( Buttons & PadButtons.Cross ) == PadButtons.Cross;
			}
		}

		public bool CircleButton
		{
			get
			{
				return ( Buttons & PadButtons.Circle ) == PadButtons.Circle;
			}
		}

		public bool SquareButton
		{
			get
			{
				return ( Buttons & PadButtons.Square ) == PadButtons.Square;
			}
		}

		public bool TriangleButton
		{
			get
			{
				return ( Buttons & PadButtons.Triangle ) == PadButtons.Triangle;
			}
		}
	}

	enum InputEventType
	{
		Depressed,
		Released,
		Unchanged
	}

	struct InputEvent
	{
		public PadButtons Button;
		public bool IsDepressed;
		public InputEventType EventType;

		public InputEvent( PadButtons button, bool isDepressed, InputEventType eventType )
		{
			Button = button;
			IsDepressed = isDepressed;
			EventType = eventType;
		}
	}

	class InputManager
	{
		protected IInputDevice _device;
		protected InputState _lastState;
		protected List<InputEvent> _events = new List<InputEvent>();

		public InputManager( IEmulationInstance emulator, IntPtr windowHandle )
		{
			Debug.Assert( emulator != null );

			foreach( IIODriver device in emulator.IO )
			{
				IInputDevice inputDevice = device as IInputDevice;
				if( inputDevice != null )
				{
					_device = inputDevice;
					_device.WindowHandle = windowHandle;
					break;
				}
			}
		}

		public void Update()
		{
			if( _device == null )
				return;

			_device.Poll();

			InputState state = new InputState();
			state.Buttons = _device.Buttons;
			state.AnalogX = _device.AnalogX;
			state.AnalogY = _device.AnalogY;

			if( _lastState != null )
			{
				_events.Clear();

				if( state.LeftButton != _lastState.LeftButton )
					_events.Add( new InputEvent( PadButtons.DigitalLeft, state.LeftButton, ( state.LeftButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.LeftButton == true )
					_events.Add( new InputEvent( PadButtons.DigitalLeft, true, InputEventType.Unchanged ) );

				if( state.RightButton != _lastState.RightButton )
					_events.Add( new InputEvent( PadButtons.DigitalRight, state.RightButton, ( state.RightButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.RightButton == true )
					_events.Add( new InputEvent( PadButtons.DigitalRight, true, InputEventType.Unchanged ) );

				if( state.UpButton != _lastState.UpButton )
					_events.Add( new InputEvent( PadButtons.DigitalUp, state.UpButton, ( state.UpButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.UpButton == true )
					_events.Add( new InputEvent( PadButtons.DigitalUp, true, InputEventType.Unchanged ) );
				
				if( state.DownButton != _lastState.DownButton )
					_events.Add( new InputEvent( PadButtons.DigitalDown, state.DownButton, ( state.DownButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.DownButton == true )
					_events.Add( new InputEvent( PadButtons.DigitalDown, true, InputEventType.Unchanged ) );

				if( state.StartButton != _lastState.StartButton )
					_events.Add( new InputEvent( PadButtons.Start, state.StartButton, ( state.StartButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.StartButton == true )
					_events.Add( new InputEvent( PadButtons.Start, true, InputEventType.Unchanged ) );

				if( state.SelectButton != _lastState.SelectButton )
					_events.Add( new InputEvent( PadButtons.Select, state.SelectButton, ( state.SelectButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.SelectButton == true )
					_events.Add( new InputEvent( PadButtons.Select, true, InputEventType.Unchanged ) );

				if( state.HomeButton != _lastState.HomeButton )
					_events.Add( new InputEvent( PadButtons.Home, state.HomeButton, ( state.HomeButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.HomeButton == true )
					_events.Add( new InputEvent( PadButtons.Home, true, InputEventType.Unchanged ) );

				if( state.LeftTriggerButton != _lastState.LeftTriggerButton )
					_events.Add( new InputEvent( PadButtons.LeftTrigger, state.LeftTriggerButton, ( state.LeftTriggerButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.LeftTriggerButton == true )
					_events.Add( new InputEvent( PadButtons.LeftTrigger, true, InputEventType.Unchanged ) );

				if( state.RightTriggerButton != _lastState.RightTriggerButton )
					_events.Add( new InputEvent( PadButtons.RightTrigger, state.RightTriggerButton, ( state.RightTriggerButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.RightTriggerButton == true )
					_events.Add( new InputEvent( PadButtons.RightTrigger, true, InputEventType.Unchanged ) );

				if( state.CrossButton != _lastState.CrossButton )
					_events.Add( new InputEvent( PadButtons.Cross, state.CrossButton, ( state.CrossButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.CrossButton == true )
					_events.Add( new InputEvent( PadButtons.Cross, true, InputEventType.Unchanged ) );

				if( state.CircleButton != _lastState.CircleButton )
					_events.Add( new InputEvent( PadButtons.Circle, state.CircleButton, ( state.CircleButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.CircleButton == true )
					_events.Add( new InputEvent( PadButtons.Circle, true, InputEventType.Unchanged ) );

				if( state.SquareButton != _lastState.SquareButton )
					_events.Add( new InputEvent( PadButtons.Square, state.SquareButton, ( state.SquareButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.SquareButton == true )
					_events.Add( new InputEvent( PadButtons.Square, true, InputEventType.Unchanged ) );

				if( state.TriangleButton != _lastState.TriangleButton )
					_events.Add( new InputEvent( PadButtons.Triangle, state.TriangleButton, ( state.TriangleButton == true ) ? InputEventType.Depressed : InputEventType.Released ) );
				else if( state.TriangleButton == true )
					_events.Add( new InputEvent( PadButtons.Triangle, true, InputEventType.Unchanged ) );
			}
			_lastState = state;
		}

		public IList<InputEvent> Events
		{
			get
			{
				return _events;
			}
		}

		public int AnalogX
		{
			get
			{
				return _lastState.AnalogX;
			}
		}

		public int AnalogY
		{
			get
			{
				return _lastState.AnalogY;
			}
		}
	}
}
