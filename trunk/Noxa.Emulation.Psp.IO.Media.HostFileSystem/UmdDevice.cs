using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.IO.Media.FileSystem
{
	class UmdDevice : IUmdDevice
	{
		protected IEmulationInstance _emulator;
		protected ComponentParameters _parameters;
		protected string _description;
		protected MediaState _state;
		protected string _hostPath;
		protected long _capacity;
		protected long _available;

		protected MediaFolder _root;

		public UmdDevice( IEmulationInstance emulator, ComponentParameters parameters, string hostPath, long capacity )
		{
			Debug.Assert( emulator != null );
			Debug.Assert( parameters != null );
			Debug.Assert( hostPath != null );
			Debug.Assert( Directory.Exists( hostPath ) == true );
			Debug.Assert( capacity > 0 );

			_emulator = emulator;
			_parameters = parameters;
			_hostPath = hostPath;

			DirectoryInfo info = new DirectoryInfo( hostPath );
			_root = new MediaFolder( this, null, info );

			_capacity = capacity;
			long used = _root.Cache();
			_available = _capacity - used;
			if( _available < 0 )
			{
				// User gave a capacity that is too small for the size, fix it up
				while( _capacity < used )
					_capacity *= 2;
				Debug.WriteLine( string.Format( "UmdDevice: user gave capacity {0} but {1} is used; changing capacity to {2}",
					capacity, used, _capacity ) );
			}

			// Would be nice to do something with this that was official-like (serial number?)
			_description = string.Format( "UMD ({0}MB)",
				_capacity / 1024 / 1024 );
		}

		public ComponentParameters Parameters
		{
			get
			{
				return _parameters;
			}
		}

		public IEmulationInstance Emulator
		{
			get
			{
				return _emulator;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
		}

		public MediaState State
		{
			get
			{
				return _state;
			}
		}

		public MediaType MediaType
		{
			get
			{
				return MediaType.Umd;
			}
		}

		public DiscType DiscType
		{
			get
			{
				// TODO: Support other disc types
				return DiscType.Game;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public string HostPath
		{
			get
			{
				return _hostPath;
			}
		}

		public IMediaFolder Root
		{
			get
			{
				return _root;
			}
		}

		public long Capacity
		{
			get
			{
				return _capacity;
			}
		}

		public long Available
		{
			get
			{
				return _available;
			}
		}

		public void Refresh()
		{
			// TODO: Figure out a way to update space nicely WITHOUT rewalking everything
			_available = _root.CalculateSize();
		}

		public void Eject()
		{
			if( _state == MediaState.Present )
				_state = MediaState.Ejected;
			else
				_state = MediaState.Present;
		}

		public void Cleanup()
		{
			_root = null;
		}
	}
}
