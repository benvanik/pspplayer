// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging
{
	/// <summary>
	/// Interface for runtime logging.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Log a line.
		/// </summary>
		/// <param name="verbosity">The verbosity level to log the line with.</param>
		/// <param name="feature">The feature the line is associated with.</param>
		/// <param name="value">The line to write.</param>
		void WriteLine( Verbosity verbosity, Feature feature, string value );
	}
}
