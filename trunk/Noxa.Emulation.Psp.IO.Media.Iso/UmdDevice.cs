using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Noxa.Emulation.Psp.IO.Media.Iso
{
	class UmdDevice : IMediaDevice
	{
		protected IEmulationInstance _emulator;
		protected string _path;

		public UmdDevice( IEmulationInstance emulator, string path )
		{
			Debug.Assert( emulator != null );
			Debug.Assert( path != null );
			Debug.Assert( File.Exists( path ) == true );

			_emulator = emulator;
			_path = path;
		}

		public string Description
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public MediaState State
		{
			get
			{
				return MediaState.Present;
			}
		}

		public MediaType MediaType
		{
			get
			{
				return MediaType.Umd;
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
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public IMediaFolder Root
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public long Capacity
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public long Available
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public void Refresh()
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public void Eject()
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public ComponentParameters Parameters
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public IEmulationInstance Emulator
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public void Cleanup()
		{
			throw new Exception( "The method or operation is not implemented." );
		}
	}
}
