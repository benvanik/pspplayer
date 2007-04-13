// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include "XInput.h"
#define DIRECTINPUT_VERSION  0x0800
#include <dinput.h>

#include "InputDriver.h"
//#include "InputApi.h"
#include <string.h>

using namespace System::Diagnostics;
using namespace System::Reflection;
using namespace System::Text;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Input;
//using namespace Noxa::Emulation::Psp::Input::Native;

LPDIRECTINPUT8       g_pDI       = NULL; // The DirectInput object
LPDIRECTINPUTDEVICE8 g_pKeyboard = NULL; // The keyboard device

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

	if( g_pKeyboard != NULL )
	{
		g_pKeyboard->Unacquire();
		g_pKeyboard->Release();
	}
	g_pKeyboard = NULL;
	if( g_pDI != NULL )
		g_pDI->Release();
	g_pDI = NULL;

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

// ---- DirectInput ----
#pragma unmanaged
void SetupDirectInput( HWND handle )
{
	HRESULT	hr;
	if( FAILED( hr = DirectInput8Create(	GetModuleHandle( NULL ),
											DIRECTINPUT_VERSION,
											IID_IDirectInput8,
											(VOID**)&g_pDI,
											NULL ) ) )
		return;
	if( FAILED( hr = g_pDI->CreateDevice( GUID_SysKeyboard, &g_pKeyboard, NULL ) ) )
		return;
    
    // This tells DirectInput that we will be passing an array
    // of 256 bytes to IDirectInputDevice::GetDeviceState.
    if( FAILED( hr = g_pKeyboard->SetDataFormat( &c_dfDIKeyboard ) ) )
        return;
    
	hr = g_pKeyboard->SetCooperativeLevel( handle, DISCL_EXCLUSIVE | DISCL_FOREGROUND );
    
	g_pKeyboard->Acquire();
}

void PollDirectInput( int* buttons, short* analogX, short* analogY )
{
	BYTE diks[ 256 ];
	ZeroMemory( diks, sizeof( diks ) );

	HRESULT hr = g_pKeyboard->GetDeviceState( sizeof( diks ), diks );
	if( FAILED( hr ) )
	{
		// If input is lost then acquire and keep trying 
		hr = g_pKeyboard->Acquire();
		while( hr == DIERR_INPUTLOST )
			hr = g_pKeyboard->Acquire();

		// hr may be DIERR_OTHERAPPHASPRIO or other errors.  This
		// may occur when the app is minimized or in the process of 
		// switching, so just try again later
		*buttons = 0;
		*analogX = 0;
		*analogY = 0;
		return;
	}

	#define KEYDOWN( key ) ( diks[ key ] & 0x80 )

	int btns = 0;
	if( KEYDOWN( DIK_SEMICOLON ) ||
		KEYDOWN( DIK_RETURN ) )
		btns |= PADCross;
	if( KEYDOWN( DIK_APOSTROPHE ) )
		btns |= PADCircle;
	if( KEYDOWN( DIK_L ) )
		btns |= PADSquare;
	if( KEYDOWN( DIK_P ) )
		btns |= PADTriangle;
	if( KEYDOWN( DIK_Z ) )
		btns |= PADStart;
	if( KEYDOWN( DIK_X ) )
		btns |= PADSelect;
	if( KEYDOWN( DIK_C ) )
		btns |= PADHome;
	if( KEYDOWN( DIK_W ) ||
		KEYDOWN( DIK_UP ) )
		btns |= PADDigitalUp;
	if( KEYDOWN( DIK_S ) ||
		KEYDOWN( DIK_DOWN ) )
		btns |= PADDigitalDown;
	if( KEYDOWN( DIK_A ) ||
		KEYDOWN( DIK_LEFT ) )
		btns |= PADDigitalLeft;
	if( KEYDOWN( DIK_D ) ||
		KEYDOWN( DIK_RIGHT ) )
		btns |= PADDigitalRight;
	if( KEYDOWN( DIK_LSHIFT ) )
		btns |= PADLeftTrigger;
	if( KEYDOWN( DIK_RSHIFT ) )
		btns |= PADRightTrigger;
	*buttons |= btns;

	*analogX = 0;
	*analogY = 0;
}
#pragma managed

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
		{
			*buttons = 0;
			*analogX = 0;
			*analogY = 0;
			return;
		}

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
	if( _dinputSetup == false )
	{
		SetupDirectInput( ( HWND )_handle );
		_dinputSetup = true;
	}

	bool connected;
	int buttons;
	short analogX;
	short analogY;
	PollXInput( _padIndex, &connected, &buttons, &analogX, &analogY );
	_isConnected = connected;
	_analogX = analogX;
	_analogY = analogY;

	if( g_pKeyboard != NULL )
		PollDirectInput( &buttons, &analogX, &analogY );

	_buttons = ( PadButtons )buttons;
}
