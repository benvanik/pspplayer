// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	abstract class Module : IModule, IDisposable
	{
		public const int NotImplementedReturn = -1;

		protected Kernel _kernel;
		protected IMemory _memory;
		protected MemorySystem _memorySystem;
		private bool _isDisposed;

		public Module( Kernel kernel )
		{
			Debug.Assert( kernel != null );
			_kernel = kernel;

			_memory = _kernel.Memory;
			_memorySystem = _kernel.MemorySystem;
		}

		~Module()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if( _isDisposed == true )
				return;
			_isDisposed = true;

			GC.SuppressFinalize( this );

			this.Stop();
		}

		public abstract string Name
		{
			get;
		}

		public abstract void Start();
		public abstract void Stop();
	}
}
