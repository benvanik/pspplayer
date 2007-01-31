// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Noxa.Emulation.Psp.Audio;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.IO;
using Noxa.Emulation.Psp.Video;
using Noxa.Emulation.Psp.Media;
using Noxa.Emulation.Psp.Input;

namespace Noxa.Emulation.Psp
{
	public enum InstanceState
	{
		Idle,
		Running,
		Paused,
		Crashed,
		Debugging,
		Ended
	}

	public interface IEmulationInstance
	{
		IEmulationHost Host
		{
			get;
		}

		EmulationParameters Parameters
		{
			get;
		}

		IAudioDriver Audio
		{
			get;
		}

		IBios Bios
		{
			get;
		}

		ICpu Cpu
		{
			get;
		}

		ReadOnlyCollection<IIODriver> IO
		{
			get;
		}

		IInputDevice Input
		{
			get;
		}

		IMemoryStickDevice MemoryStick
		{
			get;
		}

		IUmdDevice Umd
		{
			get;
		}

		IVideoDriver Video
		{
			get;
		}

		InstanceState State
		{
			get;
		}

		event EventHandler StateChanged;

		bool Create();
		void Destroy();
		void Start( bool debugging );
		void Stop();
		void Pause();
		void Resume();
		void Restart();

		void LightReset();
	}
}
