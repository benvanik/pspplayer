using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Audio;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.IO;
using Noxa.Emulation.Psp.Video;
using System.Collections.ObjectModel;

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
		void Start();
		void Stop();
		void Pause();
		void Resume();
		void Restart();

		void LightReset();
	}
}
