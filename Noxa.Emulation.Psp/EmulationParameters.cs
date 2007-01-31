// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Noxa.Emulation.Psp
{
	public class EmulationParameters
	{
		protected bool _isLocked = false;

		protected IComponent _audio;
		protected IComponent _bios;
		protected IComponent _cpu;
		protected List<IComponent> _io = new List<IComponent>();
		protected IComponent _input;
		protected IComponent _umd;
		protected IComponent _memoryStick;
		protected IComponent _video;

		protected Dictionary<IComponent, ComponentParameters> _params = new Dictionary<IComponent, ComponentParameters>();
		protected ReadOnlyDictionary<IComponent, ComponentParameters> _readOnlyParams;

		public IComponent AudioComponent
		{
			get
			{
				return _audio;
			}
			set
			{
				Debug.Assert( _isLocked == false );
				if( _isLocked == true )
					return;
				_audio = value;
			}
		}

		public IComponent BiosComponent
		{
			get
			{
				return _bios;
			}
			set
			{
				Debug.Assert( _isLocked == false );
				if( _isLocked == true )
					return;
				_bios = value;
			}
		}

		public IComponent CpuComponent
		{
			get
			{
				return _cpu;
			}
			set
			{
				Debug.Assert( _isLocked == false );
				if( _isLocked == true )
					return;
				_cpu = value;
			}
		}

		public IList<IComponent> IOComponents
		{
			get
			{
				if( _isLocked == true )
					return _io.AsReadOnly();
				else
					return _io;
			}
		}

		public IComponent InputComponent
		{
			get
			{
				return _input;
			}
			set
			{
				Debug.Assert( _isLocked == false );
				if( _isLocked == true )
					return;
				_input = value;
			}
		}

		public IComponent UmdComponent
		{
			get
			{
				return _umd;
			}
			set
			{
				Debug.Assert( _isLocked == false );
				if( _isLocked == true )
					return;
				_umd = value;
			}
		}

		public IComponent MemoryStickComponent
		{
			get
			{
				return _memoryStick;
			}
			set
			{
				Debug.Assert( _isLocked == false );
				if( _isLocked == true )
					return;
				_memoryStick = value;
			}
		}

		public IComponent VideoComponent
		{
			get
			{
				return _video;
			}
			set
			{
				Debug.Assert( _isLocked == false );
				if( _isLocked == true )
					return;
				_video = value;
			}
		}

		public IDictionary<IComponent, ComponentParameters> Parameters
		{
			get
			{
				if( _isLocked == true )
				{
					if( _readOnlyParams == null )
						_readOnlyParams = new ReadOnlyDictionary<IComponent, ComponentParameters>( _params );
					return _readOnlyParams;
				}
				else
					return _params;
			}
		}

		public ComponentParameters this[ IComponent component ]
		{
			get
			{
				return _params[ component ];
			}
			set
			{
				Debug.Assert( _isLocked == false );
				if( _isLocked == true )
					return;
				_params[ component ] = value;
			}
		}

		public void Lock()
		{
			_isLocked = true;
		}
	}
}
