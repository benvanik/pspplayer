using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Noxa.Emulation.Psp.Audio
{
	partial class FMODDriver : IAudioDriver
	{
		private struct AudioChannel
		{
			public bool Reserved;
			public AudioFormat Format;
			public int SampleCount;
			public FMOD.Sound Sound;
			public FMOD.Channel Channel;
		};

		private AudioChannel[] Channels = new AudioChannel[8];

		private FMOD.System AudioSystem = null;

		public readonly IEmulationInstance Emu;
		private readonly ComponentParameters _parameters;

		public Type Factory { get { return typeof(FMODAudio); } }
		public IEmulationInstance Emulator { get { return this.Emu; } }
		public ComponentParameters Parameters { get { return _parameters; } }

		public FMODDriver(IEmulationInstance emulator, ComponentParameters parameters)
		{
			FMOD.RESULT result;
			uint version = 0;

			this.Emu = emulator;
			this._parameters = parameters;
			
			result = FMOD.Factory.System_Create(ref this.AudioSystem);
			Debug.Assert(result == FMOD.RESULT.OK);
			
			result = this.AudioSystem.getVersion(ref version);
			Debug.Assert(result == FMOD.RESULT.OK);
			Debug.Assert(version >= FMOD.VERSION.number);

			result = this.AudioSystem.init(32, FMOD.INITFLAG.NORMAL, (IntPtr)null);
			Debug.Assert(result == FMOD.RESULT.OK);
		}

		public void Suspend()
		{
		}

		public bool Resume()
		{
			return true;
		}

		public void Cleanup()
		{
		}

		public int ReserveChannel(int channel, int sampleCount, AudioFormat format)
		{
			Debug.Assert(channel >= -1 && channel <= 7);
			if (channel == -1)
			{
				for (int i = 0; i < 8; i++)
				{
					if (this.Channels[i].Reserved == false)
					{
						channel = i;
						break;
					}
				}

				if (channel == -1)
				{
					return -1;
				}
			}
			else
			{
				if (this.Channels[channel].Reserved == true)
				{
					return -1;
				}
			}

			this.Channels[channel].Reserved = true;
			this.Channels[channel].Format = format;
			this.Channels[channel].SampleCount = sampleCount;
			this.Channels[channel].Sound = null;
			this.Channels[channel].Channel = null;
			return channel;
		}

		public void ReleaseChannel(int channel)
		{
			Debug.Assert(channel >= 0 && channel <= 7);
			if (this.Channels[channel].Reserved == false)
			{
				return;
			}

			this.Channels[channel].Reserved = false;
			return;
		}

		public void ReleaseAllChannels()
		{
		}

		private int counter = 0;
		public void Output(int channel, IntPtr buffer, bool block, int volume)
		{
			FMOD.RESULT result;

			Debug.Assert(channel >= 0 && channel <= 7);
			if (this.Channels[channel].Reserved == false)
			{
				return;
			}
			
			if (this.Channels[channel].Channel != null)
			{
				this.Channels[channel].Channel.stop();
				this.Channels[channel].Channel = null;
			}
			
			if (this.Channels[channel].Sound != null)
			{
				this.Channels[channel].Sound.release();
				this.Channels[channel].Sound = null;
			}

			uint size = (uint)(this.Channels[channel].SampleCount * 2 * 2);
			byte[] bytes = new byte[size];
			Marshal.Copy(buffer, bytes, 0, (int)size);

			FMOD.CREATESOUNDEXINFO exinfo = new FMOD.CREATESOUNDEXINFO();
			exinfo.cbsize = Marshal.SizeOf(exinfo);
			exinfo.length = size;
			exinfo.numchannels = this.Channels[channel].Format == AudioFormat.Mono ? 1 : 2;
			exinfo.defaultfrequency = 44100;
			exinfo.format = FMOD.SOUND_FORMAT.PCM16;
			exinfo.suggestedsoundtype = FMOD.SOUND_TYPE.RAW;

			result = this.AudioSystem.createSound(bytes, FMOD.MODE.OPENMEMORY | FMOD.MODE.OPENRAW, ref exinfo, ref this.Channels[channel].Sound);
			Debug.Assert(result == FMOD.RESULT.OK);

			result = this.AudioSystem.playSound(FMOD.CHANNELINDEX.REUSE, this.Channels[channel].Sound, false, ref this.Channels[channel].Channel);
			//Debug.Assert(result == FMOD.RESULT.OK);

			/*
			using (FileStream stream = File.OpenWrite(String.Format("sound_test_{0}.bin", counter++)))
			using (BinaryWriter writer = new BinaryWriter(stream))
			{
				writer.Write(bytes);
			}
			 * */
		}

		public void Output(int channel, IntPtr buffer, bool block, int leftVolume, int rightVolume)
		{
			using (FileStream stream = File.OpenWrite(String.Format("sound_test_{0}.bin", counter++)))
			using (BinaryWriter writer = new BinaryWriter(stream))
			{
				byte[] bytes = new byte[512];
				Marshal.Copy(buffer, bytes, 0, 512);
				writer.Write(bytes);
			}
		}
	}
}
