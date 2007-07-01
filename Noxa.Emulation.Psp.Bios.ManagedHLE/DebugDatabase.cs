// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging.DebugData;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	class DebugDatabase : IDebugDatabase
	{
		private List<Method> _methods;
		private List<Variable> _variables;
		private RangedLinkedList<Method> _methodLookup;
		private RangedLinkedList<Variable> _variableLookup;

		public DebugDatabase()
		{
			_methods = new List<Method>( 1024 );
			_variables = new List<Variable>( 1024 );
			_methodLookup = new RangedLinkedList<Method>( new Range( ( int )MemorySystem.MainMemoryBase, ( int )MemorySystem.MainMemorySize ) );
			_variableLookup = new RangedLinkedList<Variable>( new Range( ( int )MemorySystem.MainMemoryBase, ( int )MemorySystem.MainMemorySize ) );
		}

		public void Clear()
		{
			_methods.Clear();
			_variables.Clear();
			_methodLookup.Clear();
			_variableLookup.Clear();
		}

		#region Methods

		public void AddMethod( Method method )
		{
			_methods.Add( method );
			_methodLookup.Add( new Range( ( int )( method.Address - MemorySystem.MainMemoryBase ), ( int )method.Length ), method );
		}

		public Method[] GetMethods()
		{
			return _methods.ToArray();
		}

		public Method[] GetMethods( MethodType methodType )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public Method FindMethod( uint address )
		{
			return _methodLookup[ ( int )( address - MemorySystem.MainMemoryBase ) ];
		}

		#endregion

		#region Variables

		public void AddVariable( Variable variable )
		{
			_variables.Add( variable );
			_variableLookup.Add( new Range( ( int )( variable.Address - MemorySystem.MainMemoryBase ), ( int )variable.Length ), variable );
		}

		public Variable[] GetVariables()
		{
			return _variables.ToArray();
		}

		public Variable FindVariable( uint address )
		{
			return _variableLookup[ ( int )( address - MemorySystem.MainMemoryBase ) ];
		}

		#endregion
	}
}
