// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.IO.Media
{
	public enum MediaItemType
	{
		File,
		Folder
	}

	public enum MediaItemAttributes
	{
		Normal = 0,
		ReadOnly = 1,
		Hidden = 2,
	}

	public interface IMediaItem
	{
		string Name
		{
			get;
			set;
		}

		IMediaDevice Device
		{
			get;
		}

		IMediaFolder Parent
		{
			get;
		}

		string AbsolutePath
		{
			get;
		}

		MediaItemAttributes Attributes
		{
			get;
			set;
		}

		DateTime CreationTime
		{
			get;
			set;
		}

		DateTime ModificationTime
		{
			get;
			set;
		}

		DateTime AccessTime
		{
			get;
			set;
		}

		bool IsSymbolicLink
		{
			get;
		}

		bool MoveTo( IMediaFolder destination );
		bool CopyTo( IMediaFolder destination );
		void Delete();
	}
}
