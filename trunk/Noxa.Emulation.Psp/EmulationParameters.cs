// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp
{
	/// <summary>
	/// A set of selected components and their parameters.
	/// </summary>
	public class EmulationParameters
	{
		private bool _isLocked = false;

		private IComponent _audio;
		private IComponent _bios;
		private IComponent _cpu;
		private List<IComponent> _io = new List<IComponent>();
		private IComponent _input;
		private IComponent _umd;
		private IComponent _memoryStick;
		private IComponent _video;

		private Dictionary<IComponent, ComponentParameters> _params = new Dictionary<IComponent, ComponentParameters>();
		private ReadOnlyDictionary<IComponent, ComponentParameters> _readOnlyParams;

		/// <summary>
		/// The selected audio component.
		/// </summary>
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

		/// <summary>
		/// The selected BIOS component.
		/// </summary>
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

		/// <summary>
		/// The selected CPU component.
		/// </summary>
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

		/// <summary>
		/// A list of selected IO components.
		/// </summary>
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

		/// <summary>
		/// The selected input component.
		/// </summary>
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

		/// <summary>
		/// The selected UMD device component.
		/// </summary>
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

		/// <summary>
		/// The selected Memory Stick device component.
		/// </summary>
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

		/// <summary>
		/// The selected video component.
		/// </summary>
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

		/// <summary>
		/// The parameter collection indexed by component.
		/// </summary>
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

		/// <summary>
		/// Get the parameters for the given component.
		/// </summary>
		/// <param name="component">Component type whose parameters are to be retrieved.</param>
		/// <returns>The parameters for the component or <c>null</c> if they were not found.</returns>
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

		/// <summary>
		/// Make the parameter collection read-only.
		/// </summary>
		public void Lock()
		{
			_isLocked = true;
		}
	}
}
