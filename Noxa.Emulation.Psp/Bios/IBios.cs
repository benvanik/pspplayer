using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios
{
	#region BiosStubAttribute

	[global::System.AttributeUsage( AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
	public sealed class BiosStubAttribute : Attribute
	{
		private readonly uint _nid;
		private readonly string _name;
		private readonly bool _hasReturn;
		private readonly int _parameterCount;

		// This is a positional argument.
		public BiosStubAttribute( uint nid, string name, bool hasReturn, int parameterCount )
		{
			_nid = nid;
			_name = name;
			_hasReturn = hasReturn;
			_parameterCount = parameterCount;
		}

		public uint NID
		{
			get
			{
				return _nid;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public bool HasReturn
		{
			get
			{
				return _hasReturn;
			}
		}

		public int ParameterCount
		{
			get
			{
				return _parameterCount;
			}
		}
	}

	#endregion

	#region BiosStubIncompleteAttribute

	[global::System.AttributeUsage( AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
	public sealed class BiosStubIncompleteAttribute : Attribute
	{
		public BiosStubIncompleteAttribute()
		{
		}
	}

	#endregion

	public delegate int BiosStubDelegate( IMemory memory, int a0, int a1, int a2, int a3, int sp );

	public class BiosFunction
	{
		public IModule Module;
		public bool IsImplemented;
		public uint NID;
		public string Name;
		public BiosStubDelegate Target;
		public bool HasReturn;
		public int ParameterCount;

		// Could have other stuff here

		public BiosFunction( IModule module, bool isImplemented, uint nid, string name, BiosStubDelegate target, bool hasReturn, int parameterCount )
		{
			this.Module = module;
			this.IsImplemented = isImplemented;
			this.NID = nid;
			this.Name = name;
			this.Target = target;
			this.HasReturn = hasReturn;
			this.ParameterCount = parameterCount;
		}
	}

	public interface IBios : IComponentInstance
	{
		IKernel Kernel
		{
			get;
		}

		IModule[] Modules
		{
			get;
		}

		BiosFunction[] Functions
		{
			get;
		}

		IModule FindModule( string name );
		BiosFunction FindFunction( uint nid );

		void RegisterFunction( BiosFunction function );
		void UnregisterFunction( uint nid );
	}
}
