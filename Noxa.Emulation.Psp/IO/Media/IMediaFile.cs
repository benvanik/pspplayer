// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Noxa.Emulation.Psp.IO.Media
{
	public enum MediaFileMode
	{
		Normal,
		Append,
		Truncate
	}

	public enum MediaFileAccess
	{
		Read,
		Write,
		ReadWrite
	}

	public interface IMediaFile : IMediaItem
	{
		long Length
		{
			get;
		}

		Stream OpenRead();
		Stream Open( MediaFileMode mode, MediaFileAccess access );
	}
}
