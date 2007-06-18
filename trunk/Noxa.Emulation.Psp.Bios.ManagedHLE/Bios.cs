// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Games;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	partial class Bios : IBios
	{
		public ComponentParameters _parameters;
		public IEmulationInstance _emulator;
		
		public Loader _loader;
		public Kernel _kernel;

		public List<BiosModule> _metaModules;
		public Dictionary<string, BiosModule> _metaModuleLookup;
		public List<BiosFunction> _functions;
		public Dictionary<uint, BiosFunction> _functionLookup;
		public List<Module> _modules;
		public Dictionary<uint, IntPtr> _nativePointerLookup;

		public GameInformation _game;
		public Stream _bootStream;
		public AutoResetEvent _gameSetEvent;
		private bool _loaded;

		#region Properties

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
				return typeof( MHLE );
			}
		}

		public bool SpeedLocked
		{
			get
			{
				return _kernel.SpeedLocked;
			}
			set
			{
				_kernel.SpeedLocked = value;
			}
		}

		public ILoader Loader
		{
			get
			{
				return _loader;
			}
		}

		#endregion

		#region Modules and Functions

		public BiosModule[] Modules
		{
			get
			{
				return _metaModules.ToArray();
			}
		}

		public BiosFunction[] Functions
		{
			get
			{
				return _functions.ToArray();
			}
		}

		public BiosModule FindModule( string name )
		{
			BiosModule module;
			if( _metaModuleLookup.TryGetValue( name, out module ) == true )
				return module;
			return null;
		}

		public BiosFunction FindFunction( uint nid )
		{
			BiosFunction function;
			if( _functionLookup.TryGetValue( nid, out function ) == true )
				return function;
			return null;
		}

		public BiosFunction FindFunction( BiosFunctionToken token )
		{
			// It's possible that a NID could be 0x0, but whatever :)
			// It's also possible that there are the same NID in multiple modules.... whatever
			if( token.NID != 0x0 )
			{
				return this.FindFunction( token.NID );
			}
			else
			{
				BiosModule module = this.FindModule( token.ModuleName );
				if( module == null )
					return null;
				// This is lame - if this is used a lot, we need a better lookup!
				foreach( StubExport export in module.Exports )
				{
					BiosFunction target = this.FindFunction( export.NID );
					if( ( target != null ) &&
						( target.Name == token.MethodName ) )
						return target;
				}
				return null;
			}
		}

		private void GatherModulesAndFunctions()
		{
			int moduleCount = 0;
			int functionCount = 0;
			int implementedCount = 0;

			_metaModules = new List<BiosModule>();
			_metaModuleLookup = new Dictionary<string, BiosModule>();
			_functions = new List<BiosFunction>();
			_functionLookup = new Dictionary<uint, BiosFunction>();
			_modules = new List<Module>();
			_nativePointerLookup = new Dictionary<uint, IntPtr>();

			foreach( Type type in Assembly.GetCallingAssembly().GetTypes() )
			{
				if( type.BaseType.Equals( typeof( Module ) ) == false )
					continue;
				if( type.IsAbstract == true )
					continue;
				
				Module module = ( Module )Activator.CreateInstance( type, _kernel );
				Debug.Assert( module != null );
				if( module == null )
					continue;

				BiosModuleAliasAttribute[] aliases = ( BiosModuleAliasAttribute[] )module.GetType().GetCustomAttributes( typeof( BiosModuleAliasAttribute ), false );
				
				_modules.Add( module );
				
				BiosModule metaModule = new BiosModule( module.Name );
				_metaModules.Add( metaModule );
				_metaModuleLookup.Add( metaModule.Name, metaModule );
				foreach( BiosModuleAliasAttribute alias in aliases )
					_metaModuleLookup.Add( alias.Alias, metaModule );

				moduleCount++;

				foreach( MethodInfo mi in type.GetMethods() )
				{
					object[] attrs = mi.GetCustomAttributes( typeof( BiosFunctionAttribute ), false );
					if( attrs.Length == 0 )
						continue;
					BiosFunctionAttribute attr = ( BiosFunctionAttribute )attrs[ 0 ];

					bool isImplemented = ( mi.GetCustomAttributes( typeof( NotImplementedAttribute ), false ).Length == 0 );
					bool isStateless = ( mi.GetCustomAttributes( typeof( StatelessAttribute ), false ).Length > 0 );
					bool nativeImplSuggested = ( mi.GetCustomAttributes( typeof( SuggestNativeAttribute ), false ).Length > 0 );
					bool dontTrace = ( mi.GetCustomAttributes( typeof( DontTraceAttribute ), false ).Length > 0 );

					functionCount++;

					IntPtr nativePointer = IntPtr.Zero;
					if( isImplemented == true )
					{
					    implementedCount++;
					//    void* nativePtr = module.QueryNativePointer( attr.NID );
					//    if( nativePtr != 0x0 )
					//        nativePointer = IntPtr( nativePtr );
					}

					BiosFunction function = new BiosFunction(
						metaModule, module,
						attr.NID, attr.Name,
						isImplemented, isStateless,
						nativeImplSuggested, dontTrace,
						mi, nativePointer );
					this.RegisterFunction( function );
				}
			}

			Log.WriteLine( Verbosity.Normal, Feature.Bios, "found {0} functions in {1} modules. {2} ({3}%) implemented", functionCount, moduleCount, implementedCount, ( implementedCount / ( float )functionCount ) * 100.0f );
		}

		public void RegisterFunction( BiosFunction function )
		{
			Debug.Assert( function != null );
			if( _functionLookup.ContainsKey( function.NID ) == true )
			{
				Log.WriteLine( Verbosity.Normal, Feature.Bios, "RegisterFunction: NID 0x{0:X8} already registered", function.NID );
				return;
			}
			_functionLookup.Add( function.NID, function );
			_functions.Add( function );
		}

		#endregion

		public GameInformation Game
		{
			get
			{
				return _game;
			}
			set
			{
				if( _game != null )
				{
					Debug.Assert( value == null );
					_game = null;
					return;
				}
				_game = value;
				_kernel.StartGame();
			}
		}

		public Stream BootStream
		{
			get
			{
				return _bootStream;
			}
			set
			{
				_bootStream = value;
			}
		}

		public Bios( IEmulationInstance emulator, ComponentParameters parameters )
		{
			Debug.Assert( emulator != null );
			Debug.Assert( parameters != null );
			_emulator = emulator;
			_parameters = parameters;

			_kernel = new Kernel( this );
			_loader = new Loader( this );

			_gameSetEvent = new AutoResetEvent( false );

			this.GatherModulesAndFunctions();
		}

		public LoadResults Load()
		{
			if( _game == null )
				_gameSetEvent.WaitOne();
			LoadResults results = _kernel.LoadGame();

			_loaded = true;
			_gameSetEvent.Set();

			return results;
		}

		public void WaitUntilLoaded()
		{
			if( _loaded == true )
				return;
			_gameSetEvent.WaitOne();
		}

		public void Execute()
		{
			if( _loaded == false )
				this.WaitUntilLoaded();
			_kernel.Execute();
		}

		public void Cleanup()
		{
		}
	}
}
