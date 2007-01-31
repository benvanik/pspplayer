using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Noxa.Emulation.Psp.IO.Input.XInput
{
	static class NativeXInput
	{
		public const short LeftThumbDeadZone = 0x1ea9;
		public const short RightThumbDeadZone = 0x21f1;
		public const byte TriggerThreshold = 30;

		[DllImport( "xinput1_1.dll" )]
		public static extern void XInputEnable( bool enable );

		[DllImport( "xinput1_1.dll" )]
		public static extern uint XInputGetState( int userIndex, ref State state );

		[DllImport( "xinput1_1.dll" )]
		public static extern uint XInputSetState( int userIndex, ref Vibration vibration );

		[StructLayout( LayoutKind.Sequential )]
		public struct State
		{
			public int PacketNumber;
			public GamePadState GamePad;
			public bool IsConnected;
		}

		[Flags]
		public enum GamePadButtons : short
		{
			A = 0x1000,
			B = 0x2000,
			Back = 0x20,
			Down = 2,
			Left = 4,
			LeftShoulder = 0x100,
			LeftThumb = 0x40,
			Right = 8,
			RightShoulder = 0x200,
			RightThumb = 0x80,
			Start = 0x10,
			Up = 1,
			X = 0x4000,
			Y = -32768
		}

		[StructLayout( LayoutKind.Sequential )]
		public struct GamePadState
		{
			public GamePadButtons Buttons;
			public byte LeftTrigger;
			public byte RightTrigger;
			public short LeftThumbX;
			public short LeftThumbY;
			public short RightThumbX;
			public short RightThumbY;

			public bool UpButton { get { return ( Buttons & GamePadButtons.Up ) != 0; } }
			public bool DownButton { get { return ( Buttons & GamePadButtons.Down ) != 0; } }
			public bool LeftButton { get { return ( Buttons & GamePadButtons.Left ) != 0; } }
			public bool RightButton { get { return ( Buttons & GamePadButtons.Right ) != 0; } }
			public bool StartButton { get { return ( Buttons & GamePadButtons.Start ) != 0; } }
			public bool BackButton { get { return ( Buttons & GamePadButtons.Back ) != 0; } }
			public bool LeftThumbButton { get { return ( Buttons & GamePadButtons.LeftThumb ) != 0; } }
			public bool RightThumbButton { get { return ( Buttons & GamePadButtons.RightThumb ) != 0; } }
			public bool LeftShoulderButton { get { return ( Buttons & GamePadButtons.LeftShoulder ) != 0; } }
			public bool RightShoulderButton { get { return ( Buttons & GamePadButtons.RightShoulder ) != 0; } }
			public bool AButton { get { return ( Buttons & GamePadButtons.A ) != 0; } }
			public bool BButton { get { return ( Buttons & GamePadButtons.B ) != 0; } }
			public bool XButton { get { return ( Buttons & GamePadButtons.X ) != 0; } }
			public bool YButton { get { return ( Buttons & GamePadButtons.Y ) != 0; } }
		}

		[StructLayout( LayoutKind.Sequential )]
		public struct Vibration
		{
			public short LeftMotorSpeed;
			public short RightMotorSpeed;
		}

		public static bool LibraryExists
		{
			get
			{
				try
				{
					State state = new State();
					XInputGetState( 0, ref state );
					return true;
				}
				catch
				{
					return false;
				}
			}
		}
	}
}
