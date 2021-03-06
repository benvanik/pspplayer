// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa
{
	public interface ISettingsType
	{
		// Also need a (string, int, bool) constructor where string is the value from Serialize(), int is a depth in the save tree

		string Serialize( int depth );
	}
}
