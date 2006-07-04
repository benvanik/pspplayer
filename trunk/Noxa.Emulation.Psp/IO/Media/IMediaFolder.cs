using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.IO.Media
{
	public interface IMediaFolder : IMediaItem, IEnumerable<IMediaItem>
	{
		IMediaItem[] Items
		{
			get;
		}

		IMediaItem this[ string name ]
		{
			get;
		}

		IMediaItem Find( string path );
		IMediaFolder FindFolder( string path );
		IMediaFile FindFile( string path );

		IMediaItem CreateSymbolicLink( string name, MediaItemType type );
		IMediaFolder CreateFolder( string name );
		IMediaFile CreateFile( string name );
		bool MoveItem( string name, IMediaFolder destination );
		bool CopyItem( string name, IMediaFolder destination );
		void DeleteItem( string name );
	}
}
