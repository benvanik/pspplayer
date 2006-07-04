using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp
{
	public class EmulationParameters
	{
		protected bool _isLocked = false;

		protected IComponent _audio;
		protected IComponent _bios;
		protected IComponent _cpu;
		protected List<IComponent> _io = new List<IComponent>();
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

		public IComponent VideoComponent
		{
			get
			{
				return _video;
			}
			set
			{
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
