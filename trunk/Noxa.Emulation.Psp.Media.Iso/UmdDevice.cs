// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Noxa.Emulation.Psp.Media.Iso
{
	partial class UmdDevice : IUmdDevice
	{
		protected IEmulationInstance _emulator;
		protected ComponentParameters _parameters;
		protected string _description;
		protected MediaState _state;
		protected string _hostPath;
		protected long _capacity;

		protected MediaFolder _root;

		public UmdDevice( IEmulationInstance emulator, ComponentParameters parameters )
		{
			Debug.Assert( emulator != null );
			Debug.Assert( parameters != null );

			_emulator = emulator;
			_parameters = parameters;

			_state = MediaState.Ejected;
		}

		public UmdDevice( IEmulationInstance emulator, ComponentParameters parameters, string hostPath )
			: this( emulator, parameters )
		{
			Debug.Assert( hostPath != null );
			Debug.Assert( File.Exists( hostPath ) == true );

			this.Load( hostPath );
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

		public Type Factory
		{
			get
			{
				return typeof( IsoFileSystem );
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
				return 0;
			}
		}

		public bool Load( string path )
		{
			_hostPath = path;

			FileInfo fi = new FileInfo( path );
			_capacity = fi.Length;

			_root = ParseIsoFileSystem( path );
			Debug.Assert( _root != null );
			_root.CalculateSize();

			// Would be nice to do something with this that was official-like (serial number?)
			_description = string.Format( "UMD ({0}MB)",
				_capacity / 1024 / 1024 );

			_state = MediaState.Present;

			return true;
		}

		public void Refresh()
		{
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

		internal Stream OpenStream( long position, long length )
		{
			Stream fileStream = File.Open( _hostPath, FileMode.Open, FileAccess.Read, FileShare.Read );
			fileStream.Position = position;

			return new IsoStream( fileStream, position, length );
		}
	}
}
