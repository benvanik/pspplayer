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
		/// The number of methods in the database.
		/// </summary>
		int MethodCount
		{
			get;
		}

		/// <summary>
		/// The number of variables in the database.
		/// </summary>
		int VariableCount
		{
			get;
		}

		/// <summary>
		/// Clear the debug database.
		/// </summary>
		void Clear();

		/// <summary>
		/// Begin batch addition of symbols.
		/// </summary>
		void BeginUpdate();

		/// <summary>
		/// End batch addition of symbols.
		/// </summary>
		void EndUpdate();

		/// <summary>
		/// Add a <see cref="Symbol"/> to the debug database.
		/// </summary>
		/// <param name="symbol">The <see cref="Symbol"/> to add.</param>
		void AddSymbol( Symbol symbol );

		/// <summary>
		/// Find a specific <see cref="Symbol"/> containing the given address.
		/// </summary>
		/// <param name="address">An address contained within the <see cref="Symbol"/> to find.</param>
		/// <returns>The <see cref="Symbol"/> if found, otherwise <c>null</c>.</returns>
		Symbol FindSymbol( uint address );

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
		/// Get a list of all <see cref="Variable"/> instances in the database.
		/// </summary>
		/// <returns>A list of <see cref="Variable"/> instances.</returns>
		Variable[] GetVariables();
	}
}
