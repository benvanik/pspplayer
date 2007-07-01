// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Debugging.DebugData
{
	/// <summary>
	/// Debug database.
	/// </summary>
	public interface IDebugDatabase
	{
		/// <summary>
		/// Clear the debug database.
		/// </summary>
		void Clear();

		/// <summary>
		/// Add a <see cref="Method"/> to the debug database.
		/// </summary>
		/// <param name="method">The <see cref="Method"/> to add.</param>
		void AddMethod( Method method );

		/// <summary>
		/// Get a list of all <see cref="Method"/> instances in the database.
		/// </summary>
		/// <returns>A list of <see cref="Method"/> instances.</returns>
		Method[] GetMethods();

		/// <summary>
		/// Get a list of all <see cref="Method"/> instances matching the given <paramref name="methodType"/> in the database.
		/// </summary>
		/// <param name="methodType">The <see cref="MethodType"/> to match against.</param>
		/// <returns>A list of <see cref="Method"/> instances of the given <paramref name="methodType"/>.</returns>
		Method[] GetMethods( MethodType methodType );

		/// <summary>
		/// Find a specific <see cref="Method"/> containing the given address.
		/// </summary>
		/// <param name="address">An address contained within the <see cref="Method"/> to find.</param>
		/// <returns>The <see cref="Method"/> if found, otherwise <c>null</c>.</returns>
		Method FindMethod( uint address );

		/// <summary>
		/// Add a <see cref="Variable"/> to the debug database.
		/// </summary>
		/// <param name="variable">The <see cref="Variable"/> to add.</param>
		void AddVariable( Variable variable );

		/// <summary>
		/// Get a list of all <see cref="Variable"/> instances in the database.
		/// </summary>
		/// <returns>A list of <see cref="Variable"/> instances.</returns>
		Variable[] GetVariables();

		/// <summary>
		/// Find a specific <see cref="Variable"/> containing the given address.
		/// </summary>
		/// <param name="address">An address contained within the <see cref="Variable"/> to find.</param>
		/// <returns>The <see cref="Variable"/> if found, otherwise <c>null</c>.</returns>
		Variable FindVariable( uint address );
	}
}
