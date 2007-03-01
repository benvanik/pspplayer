// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Cpu;
using System.Reflection;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Bios
{
	#region IncompleteAttribute

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

		public object Target;
		public MethodInfo MethodInfo;

		public int ParameterCount;
		public bool UsesMemorySystem;
		public bool HasReturn;
		public bool DoubleWordReturn;

		public BiosFunction( IModule module, uint nid, string name, bool isImplemented, bool isStateless, object target, MethodInfo methodInfo )
		{
			this.Module = module;
			this.NID = nid;
			this.Name = name;
			this.IsImplemented = isImplemented;
			this.IsStateless = isStateless;

			this.Target = target;
			this.MethodInfo = methodInfo;
			
			this.HasReturn = ( methodInfo.ReturnType != null );
			if( this.HasReturn == true )
				this.DoubleWordReturn = ( methodInfo.ReturnType == typeof( long ) );
			
			ParameterInfo[] ps = methodInfo.GetParameters();
			if( ps.Length > 0 )
			{
				this.ParameterCount = ps.Length;
				if( ps[ 0 ].ParameterType == typeof( IMemory ) )
				{
					this.ParameterCount--;
					this.UsesMemorySystem = true;
				}

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
