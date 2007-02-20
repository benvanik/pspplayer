// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using Noxa.Emulation.Psp.Bios.GenericHle.Modules;

namespace Noxa.Emulation.Psp.Bios.GenericHle
{
	class HleInstance : IComponentInstance, IBios
	{
		protected IEmulationInstance _emulator;
		protected ComponentParameters _parameters;
		protected List<IModule> _moduleList = new List<IModule>();
		protected Dictionary<string, IModule> _modules = new Dictionary<string, IModule>();
		protected List<BiosFunction> _functionList = new List<BiosFunction>();
		protected Dictionary<uint, BiosFunction> _functions = new Dictionary<uint, BiosFunction>();

		protected Kernel _kernel;

		public HleInstance( IEmulationInstance emulator, ComponentParameters parameters )
		{
			Debug.Assert( emulator != null );
			Debug.Assert( parameters != null );

			_emulator = emulator;
			_parameters = parameters;

			_kernel = new Kernel( this );

			// Find modules
			foreach( Type type in Assembly.GetCallingAssembly().GetTypes() )
			{
				if( type.GetInterface( "Noxa.Emulation.Psp.Bios.IModule" ) == null )
					continue;

				IModule module = ( IModule )Activator.CreateInstance( type, this );
				_moduleList.Add( module );
				_modules.Add( module.Name, module );

				foreach( MethodInfo mi in type.GetMethods() )
				{
					object[] attrs = mi.GetCustomAttributes( typeof( BiosStubAttribute ), false );
					if( attrs.Length == 0 )
						continue;
					BiosStubAttribute attr = attrs[ 0 ] as BiosStubAttribute;

					bool isImplemented = true;
					if( mi.GetCustomAttributes( typeof( BiosStubIncompleteAttribute ), false ).Length > 0 )
						isImplemented = false;

					bool isOverridable = false;
					if( mi.GetCustomAttributes( typeof( BiosStubOverridableAttribute ), false ).Length > 0 )
						isOverridable = true;
					
					BiosStubDelegate del = Delegate.CreateDelegate( typeof( BiosStubDelegate ), module, mi ) as BiosStubDelegate;

					this.RegisterFunction( new BiosFunction( module,
						isImplemented, isOverridable,
						attr.NID, attr.Name, del, attr.HasReturn, attr.ParameterCount ) );
				}
			}
		}

		public ComponentParameters Parameters
		{
			get
			{
				return _parameters;
			}
		}

		public IEmulationInstance Emulator
		{
			get
			{
				return _emulator;
			}
		}

		public Type Factory
		{
			get
			{
				return typeof( GenericHleBios );
			}
		}

		public IKernel Kernel
		{
			get
			{
				return _kernel;
			}
		}

		public void Cleanup()
		{
			foreach( IModule module in _moduleList )
			{
				if( module is IStatefulModule )
				{
					IStatefulModule stateful = module as IStatefulModule;
					stateful.Stop();
					stateful.Clear();
				}
			}
		}

		public IModule[] Modules
		{
			get
			{
				return _moduleList.ToArray();
			}
		}

		public BiosFunction[] Functions
		{
			get
			{
				return _functionList.ToArray();
			}
		}

		public void ClearModules()
		{
			foreach( IModule module in _moduleList )
			{
				if( module is IStatefulModule )
				{
					IStatefulModule stateful = module as IStatefulModule;
					stateful.Clear();
				}
			}
		}

		public void StartModules()
		{
			foreach( IModule module in _moduleList )
			{
				if( module is IStatefulModule )
				{
					IStatefulModule stateful = module as IStatefulModule;
					stateful.Start();
				}
			}
		}

		public void StopModules()
		{
			foreach( IModule module in _moduleList )
			{
				if( module is IStatefulModule )
				{
					IStatefulModule stateful = module as IStatefulModule;
					stateful.Stop();
				}
			}
		}

		public IModule FindModule( string name )
		{
			if( _modules.ContainsKey( name ) == true )
				return _modules[ name ];
			return null;
		}

		public BiosFunction FindFunction( uint nid )
		{
			if( _functions.ContainsKey( nid ) == true )
				return _functions[ nid ];
			return null;
		}

		public void RegisterFunction( BiosFunction function )
		{
			Debug.Assert( function != null );
			if( _functions.ContainsKey( function.NID ) == true )
			{
				Debug.WriteLine( string.Format( "HLE RegisterFunction: NID 0x{X0} already registered", function.NID ) );
				return;
			}
			_functions.Add( function.NID, function );
			_functionList.Add( function );
		}

		public void UnregisterFunction( uint nid )
		{
			if( _functions.ContainsKey( nid ) == false )
				return;
			BiosFunction function = _functions[ nid ];
			_functions.Remove( nid );
			_functionList.Remove( function );
		}
	}
}
