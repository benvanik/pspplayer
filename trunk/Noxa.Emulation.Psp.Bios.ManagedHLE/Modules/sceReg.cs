// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

using Noxa.Utilities;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Media;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	unsafe class sceReg : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceReg";
			}
		}

		#endregion

		#region State Management

		private void LoadXmlDirectory( XPathNavigator node, RegistryKey key )
		{
			XPathNodeIterator nodes = node.Select( "*" );

			while( nodes.MoveNext() )
			{
				if( nodes.Current.Name == "integer" )
				{
					string name = nodes.Current.GetAttribute( "name", "" );
					int value = nodes.Current.ValueAsInt;
					Debug.Assert( name != "" );
					//Log.WriteLine(Verbosity.Verbose, Feature.Bios, "xml: adding integer {0} to {1}: {2}", name, key.Name, value);
					key.Children.Add( new RegistryKey( name, value ) );
				}
				else if( nodes.Current.Name == "string" )
				{
					string name = nodes.Current.GetAttribute( "name", "" );
					string value = nodes.Current.Value;
					Debug.Assert( name != "" );
					//Log.WriteLine(Verbosity.Verbose, Feature.Bios, "xml: adding string {0} to {1}: {2}", name, key.Name, value);
					key.Children.Add( new RegistryKey( name, value ) );
				}
				else if( nodes.Current.Name == "binary" )
				{
					string name = nodes.Current.GetAttribute( "name", "" );
					string value = nodes.Current.Value;
					Debug.Assert( name != "" );
					//Debug.Assert( false );
					key.Children.Add( new RegistryKey( name, value ) );
				}
				else if( nodes.Current.Name == "directory" )
				{
					string name = nodes.Current.GetAttribute( "name", "" );
					Debug.Assert( name != "" );
					//Log.WriteLine(Verbosity.Verbose, Feature.Bios, "xml: adding directory {0} to {1}", name, key.Name);
					RegistryKey value = new RegistryKey( name );
					this.LoadXmlDirectory( nodes.Current, value );
					key.Children.Add( value );
				}
				else
				{
					Debug.Assert( false );
				}
			}
		}

		protected void LoadXml()
		{
			IMediaFile file = ( IMediaFile )_kernel.FindPath( "ms0:/registry.xml" );
			Debug.Assert( file != null );
			if( file == null )
			{
				Log.WriteLine( Verbosity.Critical, Feature.Bios, "Registry not found - make sure you have registry.xml (and font/) on your memory stick!" );
				return;
			}

			using( Stream stream = file.Open( MediaFileMode.Normal, MediaFileAccess.Read ) )
			{
				Debug.Assert( stream != null );

				XPathDocument document = new XPathDocument( stream );
				XPathNavigator navigator = document.CreateNavigator();
				XPathNavigator node = navigator.SelectSingleNode( "/registry" );

				this.LoadXmlDirectory( node, _system );
			}
		}

		public sceReg( Kernel kernel )
			: base( kernel )
		{
			_system = new RegistryKey( "system" );
		}

		public override void Start()
		{
			this.LoadXml();
		}

		public override void Stop()
		{
		}

		#endregion

		enum RegistryKeyType
		{
			Directory = 1,
			Integer = 2,
			String = 3,
			Binary = 4,
		}

		class RegistryKey
		{
			public RegistryKeyType Type;
			public string Name;

			public List<RegistryKey> Children;
			public int ValueI;
			public string ValueS;
			public byte[] ValueB;

			public RegistryKey( string name )
			{
				this.Type = RegistryKeyType.Directory;
				this.Name = name;
				this.Children = new List<RegistryKey>();
			}

			public RegistryKey( string name, int value )
			{
				this.Type = RegistryKeyType.Integer;
				this.Name = name;
				this.ValueI = value;
			}

			public RegistryKey( string name, string value )
			{
				this.Type = RegistryKeyType.String;
				this.Name = name;
				this.ValueS = value;
			}

			public RegistryKey( string name, byte[] value )
			{
				this.Type = RegistryKeyType.Binary;
				this.Name = name;
				this.ValueB = value;
			}
		}

		enum RegistryHandleType
		{
			Registry,
			Category,
			Key,
		}

		class RegistryHandle
		{
			public RegistryHandleType Type;
			public int ID;
			public RegistryKey Key;
			public List<RegistryHandle> Handles = new List<RegistryHandle>();

			public RegistryHandle GetHandle( int id )
			{
				foreach( RegistryHandle handle in this.Handles )
				{
					if( handle.ID == id )
						return handle;
				}
				return null;
			}
		}

		private RegistryKey _system;
		private List<RegistryHandle> _handles = new List<RegistryHandle>();
		private int _lastId;

		private RegistryHandle GetHandle( int id )
		{
			foreach( RegistryHandle handle in _handles )
			{
				if( handle.ID == id )
					return handle;
			}
			return null;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9B25EDF1, "sceRegExit" )]
		public int sceRegExit() { return Module.NotImplementedReturn; }

		[Stateless]
		[BiosFunction( 0x92E41280, "sceRegOpenRegistry" )]
		// SDK location: /registry/pspreg.h:68
		// SDK declaration: int sceRegOpenRegistry(struct RegParam *reg, int mode, REGHANDLE *h);
		public int sceRegOpenRegistry( int reg, int mode, int h )
		{
			//unsigned int  regtype 		
			//char  name [256] 			//Seemingly never used, set to "/system". 
			//unsigned int  namelen 		//Length of the name. 
			//unsigned int  unk2 			//Unknown, set to 1. 
			//unsigned int  unk3 			//Unknown, set to 1. 
			// mode = 1

			string name = _kernel.ReadString( ( uint )reg + 4 );

			RegistryHandle handle = new RegistryHandle();
			handle.Type = RegistryHandleType.Registry;
			handle.ID = _lastId++;
			handle.Key = _system;
			_handles.Add( handle );

			if( h != 0 )
			{
				int* ph = ( int* )_memorySystem.TranslateMainMemory( h );
				*ph = handle.ID;
			}
			return 0;
		}

		[Stateless]
		[BiosFunction( 0xFA8A5739, "sceRegCloseRegistry" )]
		// SDK location: /registry/pspreg.h:86
		// SDK declaration: int sceRegCloseRegistry(REGHANDLE h);
		public int sceRegCloseRegistry( int h )
		{
			RegistryHandle handle = this.GetHandle( h );
			if( handle == null )
				return -1;
			else
			{
				_handles.Remove( handle );
				return 0;
			}
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDEDA92BF, "sceRegRemoveRegistry" )]
		// SDK location: /registry/pspreg.h:229
		// SDK declaration: int sceRegRemoveRegistry(struct RegParam *reg);
		public int sceRegRemoveRegistry( int reg ) { return Module.NotImplementedReturn; }

		[Stateless]
		[BiosFunction( 0x1D8A762E, "sceRegOpenCategory" )]
		// SDK location: /registry/pspreg.h:98
		// SDK declaration: int sceRegOpenCategory(REGHANDLE h, const char *name, int mode, REGHANDLE *hd);
		public int sceRegOpenCategory( int h, int name, int mode, int hd )
		{
			RegistryHandle registryHandle = this.GetHandle( h );
			if( registryHandle == null )
				return -1;

			RegistryKey key = registryHandle.Key;

			// This is probably wrong
			string categoryPath = _kernel.ReadString( ( uint )name );
			string[] path = categoryPath.Split( new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries );
			foreach( string pathElement in path )
			{
				foreach( RegistryKey child in key.Children )
				{
					if( child.Name == pathElement )
					{
						key = child;
						break;
					}
				}
			}

			RegistryHandle handle = new RegistryHandle();
			handle.Type = RegistryHandleType.Category;
			handle.ID = _lastId++;
			handle.Key = key;
			_handles.Add( handle );

			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "sceRegOpenCategory( {0}::{1} )", registryHandle.Key.Name, categoryPath );

			if( hd != 0 )
			{
				int* phd = ( int* )_memorySystem.TranslateMainMemory( hd );
				*phd = handle.ID;
			}
			return 0;
		}

		[Stateless]
		[BiosFunction( 0x0CAE832B, "sceRegCloseCategory" )]
		// SDK location: /registry/pspreg.h:117
		// SDK declaration: int sceRegCloseCategory(REGHANDLE hd);
		public int sceRegCloseCategory( int hd )
		{
			RegistryHandle handle = this.GetHandle( hd );
			if( handle == null )
				return -1;
			else
			{
				_handles.Remove( handle );
				return 0;
			}
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x39461B4D, "sceRegFlushRegistry" )]
		// SDK location: /registry/pspreg.h:77
		// SDK declaration: int sceRegFlushRegistry(REGHANDLE h);
		public int sceRegFlushRegistry( int h ) { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x0D69BF40, "sceRegFlushCategory" )]
		// SDK location: /registry/pspreg.h:126
		// SDK declaration: int sceRegFlushCategory(REGHANDLE hd);
		public int sceRegFlushCategory( int hd ) { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57641A81, "sceRegCreateKey" )]
		// SDK location: /registry/pspreg.h:220
		// SDK declaration: int sceRegCreateKey(REGHANDLE hd, const char *name, int type, SceSize size);
		public int sceRegCreateKey( int hd, int name, int type, int size ) { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x17768E14, "sceRegSetKeyValue" )]
		// SDK location: /registry/pspreg.h:187
		// SDK declaration: int sceRegSetKeyValue(REGHANDLE hd, const char *name, const void *buf, SceSize size);
		public int sceRegSetKeyValue( int hd, int name, int buf, int size ) { return Module.NotImplementedReturn; }

		[Stateless]
		[BiosFunction( 0xD4475AA8, "sceRegGetKeyInfo" )]
		// SDK location: /registry/pspreg.h:139
		// SDK declaration: int sceRegGetKeyInfo(REGHANDLE hd, const char *name, REGHANDLE *hk, unsigned int *type, SceSize *size);
		public int sceRegGetKeyInfo( int hd, int name, int hk, int type, int size )
		{
			RegistryHandle categoryHandle = this.GetHandle( hd );
			if( categoryHandle == null )
				return -1;

			string keyName = _kernel.ReadString( ( uint )name );

			Log.WriteLine( Verbosity.Verbose, Feature.Bios, "sceRegGetKeyInfo( {0}/{1} )", categoryHandle.Key.Name, keyName );

			RegistryKey key = null;
			foreach( RegistryKey child in categoryHandle.Key.Children )
			{
				if( child.Name == keyName )
				{
					key = child;
					break;
				}
			}

			Debug.Assert( key != null );
			if( key == null )
				return -1;

			RegistryHandle handle = new RegistryHandle();
			handle.Type = RegistryHandleType.Key;
			handle.ID = _lastId++;
			handle.Key = key;
			categoryHandle.Handles.Add( handle );

			int typev = ( int )key.Type;
			int sizev = 0;
			switch( key.Type )
			{
				case RegistryKeyType.Integer:
					sizev = 4;
					break;
				case RegistryKeyType.String:
					sizev = key.ValueS.Length + 1;
					break;
				case RegistryKeyType.Binary:
					sizev = key.ValueB.Length;
					break;
			}

			if( hk != 0 )
			{
				int* phk = ( int* )_memorySystem.TranslateMainMemory( hk );
				*phk = handle.ID;
			}
			if( type != 0 )
			{
				int* ptype = ( int* )_memorySystem.TranslateMainMemory( type );
				*ptype = typev;
			}
			if( size != 0 )
			{
				int* psize = ( int* )_memorySystem.TranslateMainMemory( size );
				*psize = sizev;
			}

			return 0;
		}

		[Stateless]
		[BiosFunction( 0x28A8E98A, "sceRegGetKeyValue" )]
		// SDK location: /registry/pspreg.h:163
		// SDK declaration: int sceRegGetKeyValue(REGHANDLE hd, REGHANDLE hk, void *buf, SceSize size);
		public int sceRegGetKeyValue( int hd, int hk, int buf, int size )
		{
			RegistryHandle categoryHandle = this.GetHandle( hd );
			if( categoryHandle == null )
				return -1;

			RegistryHandle keyHandle = categoryHandle.GetHandle( hk );
			if( keyHandle == null )
				return -1;

			byte* pbuf = _memorySystem.TranslateMainMemory( buf );
			switch( keyHandle.Key.Type )
			{
				case RegistryKeyType.Integer:
					*( ( int* )pbuf ) = keyHandle.Key.ValueI;
					break;
				case RegistryKeyType.String:
					_kernel.WriteString( ( uint )buf, keyHandle.Key.ValueS );
					break;
				case RegistryKeyType.Binary:
					{
						byte[] value = keyHandle.Key.ValueB;
						fixed( byte* pvalue = &value[ 0 ] )
							MemorySystem.CopyMemory( pvalue, pbuf, ( uint )value.Length );
					}
					break;
			}

			return 0;
		}

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2C0DB9DD, "sceRegGetKeysNum" )]
		// SDK location: /registry/pspreg.h:197
		// SDK declaration: int sceRegGetKeysNum(REGHANDLE hd, int *num);
		public int sceRegGetKeysNum( int hd, int num ) { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x2D211135, "sceRegGetKeys" )]
		// SDK location: /registry/pspreg.h:208
		// SDK declaration: int sceRegGetKeys(REGHANDLE hd, char *buf, int num);
		public int sceRegGetKeys( int hd, int buf, int num ) { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4CA16893, "sceRegRemoveCategory" )]
		// SDK location: /registry/pspreg.h:108
		// SDK declaration: int sceRegRemoveCategory(REGHANDLE h, const char *name);
		public int sceRegRemoveCategory( int h, int name ) { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3615BC87, "sceRegRemoveKey" )]
		public int sceRegRemoveKey() { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC5768D02, "sceRegGetKeyInfoByName" )]
		// SDK location: /registry/pspreg.h:151
		// SDK declaration: int sceRegGetKeyInfoByName(REGHANDLE hd, const char *name, unsigned int *type, SceSize *size);
		public int sceRegGetKeyInfoByName( int hd, int name, int type, int size ) { return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x30BE0259, "sceRegGetKeyValueByName" )]
		// SDK location: /registry/pspreg.h:175
		// SDK declaration: int sceRegGetKeyValueByName(REGHANDLE hd, const char *name, void *buf, SceSize size);
		public int sceRegGetKeyValueByName( int hd, int name, int buf, int size ) { return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - 6187A697 */
