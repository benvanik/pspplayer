// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	class KIntHandler : KHandle, IDisposable
	{
		public Kernel Kernel;

		public int InterruptNumber;
		public int Slot;

		public uint Address;
		public uint Argument;

		public uint CallCount;

		// Timing info?
		// total clock (int64), min clock (int64), max clock (int64)?

		private bool _isInstalled;

		public KIntHandler( Kernel kernel, int interruptNumber, int slot, uint address, uint argument )
		{
			Kernel = kernel;

			InterruptNumber = interruptNumber;
			Slot = slot;

			Address = address;
			Argument = argument;

			CallCount = 0;
		}

		~KIntHandler()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			GC.SuppressFinalize( this );

			this.Enabled = false;
		}

		public bool Enabled
		{
			get
			{
				return _isInstalled;
			}
			set
			{
				if( value == _isInstalled )
					return;

				if( value == true )
				{
					// Install
					Kernel.Cpu.RegisterInterruptHandler( InterruptNumber, Slot, Address, Argument );
				}
				else
				{
					// Uninstall
					Kernel.Cpu.UnregisterInterruptHandler( InterruptNumber, Slot );
				}
				_isInstalled = value;
			}
		}
	}
}
