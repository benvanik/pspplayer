// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include "XInput.h"

#include "InputDriver.h"
//#include "InputApi.h"
#include <string.h>

using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Input;
//using namespace Noxa::Emulation::Psp::Input::Native;

InputDriver::InputDriver( IEmulationInstance^ emulator, ComponentParameters^ parameters )
{
	GlobalDriver = this;

	_emu = emulator;
	_params = parameters;

	//_nativeInterface = malloc( sizeof( InputApi ) );
	//memset( _nativeInterface, 0, sizeof( InputApi ) );
	//this->SetupNativeInterface();

	XInputEnable( true );
}

InputDriver::~InputDriver()
{
	//this->DestroyNativeInterface();
	//SAFEFREE( _nativeInterface );
}

bool InputDriver::IsConnected::get()
{
	return _isConnected;
}

int InputDriver::AnalogX::get()
{
	return _analogY;
}

int InputDriver::AnalogY::get()
{
	return _analogX;
}

PadButtons InputDriver::Buttons::get()
{
	return _buttons;
}

void InputDriver::Cleanup()
{
	XInputEnable( false );
}

enum PadButtons_e
{
	PADNone				= 0x000000,
	PADSelect			= 0x000001,
	PADStart			= 0x000008,
	PADDigitalUp		= 0x000010,
	PADDigitalRight		= 0x000020,
	PADDigitalDown		= 0x000040,
	PADDigitalLeft		= 0x000080,
	PADLeftTrigger		= 0x000100,
	PADRightTrigger		= 0x000200,
	PADTriangle			= 0x001000,
	PADCircle			= 0x002000,
	PADCross			= 0x004000,
	PADSquare			= 0x008000,
	PADHome				= 0x010000,
	PADHold				= 0x020000,
	PADMusicNote		= 0x800000,
};

// ---- XInput ----
#pragma unmanaged
DWORD _lastPacket = 0;
void PollXInput( int padIndex, bool* connected, int* buttons, short* analogX, short* analogY )
{
	XINPUT_STATE state;
	do
	{
		DWORD ret = XInputGetState( padIndex, &state );
		*connected = ( ret != ERROR_DEVICE_NOT_CONNECTED );
		if( *connected == false )
			return;

		if( ret == ERROR_SUCCESS )
		{
			// Loop until we get the next packet
			//if( state.dwPacketNumber <= _lastPacket )
			//	continue;
			_lastPacket = state.dwPacketNumber;

			int btns = PADNone;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_A ) == XINPUT_GAMEPAD_A )
				btns |= PADCross;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_B ) == XINPUT_GAMEPAD_B )
				btns |= PADCircle;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_X ) == XINPUT_GAMEPAD_X )
				btns |= PADSquare;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_Y ) == XINPUT_GAMEPAD_Y )
				btns |= PADTriangle;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_START ) == XINPUT_GAMEPAD_START )
				btns |= PADStart;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_BACK ) == XINPUT_GAMEPAD_BACK )
				btns |= PADSelect;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_UP ) == XINPUT_GAMEPAD_DPAD_UP )
				btns |= PADDigitalUp;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_DOWN ) == XINPUT_GAMEPAD_DPAD_DOWN )
				btns |= PADDigitalDown;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_LEFT ) == XINPUT_GAMEPAD_DPAD_LEFT )
				btns |= PADDigitalLeft;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_RIGHT ) == XINPUT_GAMEPAD_DPAD_RIGHT )
				btns |= PADDigitalRight;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_LEFT_SHOULDER ) == XINPUT_GAMEPAD_LEFT_SHOULDER )
				btns |= PADLeftTrigger;
			if( ( state.Gamepad.wButtons & XINPUT_GAMEPAD_RIGHT_SHOULDER ) == XINPUT_GAMEPAD_RIGHT_SHOULDER )
				btns |= PADRightTrigger;
			*buttons = btns;

			short ax = state.Gamepad.sThumbLY;
			short ay = state.Gamepad.sThumbLX;

			if( ( ax < XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE ) &&
				( ax > -XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE ) )
				ax = 0;
			if( ( ay < XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE ) &&
				( ay > -XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE ) )
				ay = 0;

			*analogX = ax;
			*analogY = ay;

			// Done!
			break;
		}
	} while( true );
}
#pragma managed

void InputDriver::Poll()
{
	bool connected;
	int buttons;
	short analogX;
	short analogY;
	PollXInput( _padIndex, &connected, &buttons, &analogX, &analogY );
	_isConnected = connected;
	_buttons = ( PadButtons )buttons;
	_analogX = analogX;
	_analogY = analogY;
}
