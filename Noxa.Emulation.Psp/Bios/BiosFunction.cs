// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios
{
	#region NotImplementedAttribute

	[global::System.AttributeUsage( AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
	public sealed class NotImplementedAttribute : Attribute
	{
		public NotImplementedAttribute()
		{
		}
	}

	#endregion

	#region StatelessAttribute

	[global::System.AttributeUsage( AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
	public sealed class StatelessAttribute : Attribute
	{
		public StatelessAttribute()
		{
		}
	}

	#endregion

	public class BiosFunction
	{
		public IModule Module;
		public uint NID;
		public string Name;
		public bool IsImplemented;
		public bool IsStateless;

		public MethodInfo MethodInfo;
		public IntPtr NativeMethod;

		public int ParameterCount;
		public BitArray ParameterWidths;
		public bool UsesMemorySystem;

		public BiosFunction( IModule module, uint nid, string name, bool isImplemented, bool isStateless, MethodInfo methodInfo )
		{
			this.Module = module;
			this.NID = nid;
			this.Name = name;
			this.IsImplemented = isImplemented;
			this.IsStateless = isStateless;

			this.MethodInfo = methodInfo;
			
			ParameterInfo[] ps = methodInfo.GetParameters();
			if( ps.Length > 0 )
			{
				this.ParameterCount = ps.Length;
				if( ps[ 0 ].ParameterType == typeof( IMemory ) )
				{
					this.ParameterCount--;
					this.UsesMemorySystem = true;
				}

				this.ParameterWidths = new BitArray( this.ParameterCount );
				int offset = ( this.UsesMemorySystem == true ) ? -1 : 0;
				for( int n = 1; n < ps.Length; n++ )
					ParameterWidths[ n - 1 ] = ( ps[ n + offset ].ParameterType == typeof( long ) );
#if DEBUG
				// Sanity check to make sure IMemory is always the first argument
				if( this.UsesMemorySystem == false )
				{
					for( int n = 0; n < ps.Length; n++ )
						Debug.Assert( ps[ n ].ParameterType != typeof( IMemory ) );
				}
#endif
			}
		}
	}
}
