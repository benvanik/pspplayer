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
using Noxa.Emulation.Psp.Input;
using Noxa.Emulation.Psp.IO;
using Noxa.Emulation.Psp.Media;
using Noxa.Emulation.Psp.Video;

namespace Noxa.Emulation.Psp
{
	/// <summary>
	/// Describes the state of an <see cref="IEmulationInstance"/>.
	/// </summary>
	public enum InstanceState
	{
		/// <summary>
		/// Emulation has not yet started.
		/// </summary>
		Idle,

		/// <summary>
		/// Emulation is underway.
		/// </summary>
		Running,

		/// <summary>
		/// Emulation is paused.
		/// </summary>
		Paused,

		/// <summary>
		/// The emulator has crashed.
		/// </summary>
		Crashed,

		/// <summary>
		/// The emulator is being debugged.
		/// </summary>
		Debugging,

		/// <summary>
		/// Emulation has ended.
		/// </summary>
		Ended,
	}

	/// <summary>
	/// The currently executing emulator instance.
	/// </summary>
	public interface IEmulationInstance
	{
		/// <summary>
		/// The <see cref="IEmulationHost"/> that owns the instance.
		/// </summary>
		IEmulationHost Host
		{
			get;
		}

		/// <summary>
		/// The parameters used to create the instance.
		/// </summary>
		EmulationParameters Parameters
		{
			get;
		}

		/// <summary>
		/// A list of <see cref="IComponentInstance"/> instances in use by this instance.
		/// </summary>
		IComponentInstance[] Components
		{
			get;
		}

		/// <summary>
		/// The audio driver in use by the instance.
		/// </summary>
		IAudioDriver Audio
		{
			get;
		}

		/// <summary>
		/// The BIOS in use by the instance.
		/// </summary>
		IBios Bios
		{
			get;
		}

		/// <summary>
		/// The CPU in use by the instance.
		/// </summary>
		ICpu Cpu
		{
			get;
		}

		/// <summary>
		/// A list of IO drivers in use by the instance.
		/// </summary>
		ReadOnlyCollection<IIODriver> IO
		{
			get;
		}

		/// <summary>
		/// The input driver in use by the instance.
		/// </summary>
		IInputDevice Input
		{
			get;
		}

		/// <summary>
		/// The Memory Stick device in use by the instance.
		/// </summary>
		IMemoryStickDevice MemoryStick
		{
			get;
		}

		/// <summary>
		/// The UMD device in use by the instance.
		/// </summary>
		IUmdDevice Umd
		{
			get;
		}

		/// <summary>
		/// The video driver in use by the instance.
		/// </summary>
		IVideoDriver Video
		{
			get;
		}

		/// <summary>
		/// The current state of the instance.
		/// </summary>
		InstanceState State
		{
			get;
		}

		/// <summary>
		/// Fired when the current state of the instance changes.
		/// </summary>
		event EventHandler StateChanged;

		/// <summary>
		/// Create the instance and all components..
		/// </summary>
		/// <returns><c>true</c> on success; otherwise <c>false</c>.</returns>
		bool Create();

		/// <summary>
		/// Destroy the instance and all components.
		/// </summary>
		void Destroy();

		/// <summary>
		/// Start the instance.
		/// </summary>
		/// <param name="debugging"><c>true</c> to start with debugging.</param>
		void Start( bool debugging );

		/// <summary>
		/// Stop the instance.
		/// </summary>
		void Stop();

		/// <summary>
		/// Pause the currently running instance.
		/// </summary>
		void Pause();

		/// <summary>
		/// Resume the currently paused instance.
		/// </summary>
		void Resume();

		/// <summary>
		/// Restart the instance.
		/// </summary>
		void Restart();

		/// <summary>
		/// Reset the instance without destroying components.
		/// </summary>
		void LightReset();

		/// <summary>
		/// Lock the emulator to V-sync.
		/// </summary>
		void LockSpeed();

		/// <summary>
		/// Unlock the emulator from V-sync.
		/// </summary>
		void UnlockSpeed();
	}
}
