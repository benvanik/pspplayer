// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Player.Debugger.Model
{
	class CodeCache
	{
		public readonly InprocDebugger Debugger;
		public List<MethodBody> Methods;

		public int Version;

		public CodeCache( InprocDebugger debugger )
		{
			this.Debugger = debugger;
			this.Methods = new List<MethodBody>();
		}

		public void Update()
		{
			//Method[] userMethods = this.Debugger.DebugHost.Database.GetMethods( MethodType.User );
			Method[] methods = this.Debugger.DebugHost.Database.GetMethods();
			this.Methods.Clear();
			foreach( Method method in methods )
			{
				//MethodBody body = new MethodBody( method.Address, method.Length, null );
				MethodBody body = this.BuildMethodBody( method );
				// Name, etc?
				this.Methods.Add( body );
			}
			this.Methods.Sort( delegate( MethodBody a, MethodBody b )
			{
				return a.Address.CompareTo( b.Address );
			} );

			this.LinkReferences();

			this.Version++;
		}

		private void LinkReferences()
		{
			// nlogn, in theory
			foreach( MethodBody method in this.Methods )
			{
				foreach( CodeReference codeRef in method.CodeReferences )
				{
					if( codeRef.Address == 0 )
						continue;
					MethodBody other = this[ codeRef.Address ];
					if( other == null )
					{
						// Target not found?
						continue;
					}
					foreach( Instruction instruction in codeRef.References )
					{
						ExternalReference extRef = new ExternalReference();
						extRef.Method = method;
						extRef.SourceAddress = instruction.Address;
						extRef.TargetAddress = codeRef.Address;
						other.IncomingReferences.Add( extRef );
						extRef = new ExternalReference();
						extRef.Method = other;
						extRef.SourceAddress = instruction.Address;
						extRef.TargetAddress = codeRef.Address;
						method.OutgoingReferences.Add( extRef );
					}
				}
				if( method.OutgoingReferences.Count > 0 )
				{
					method.OutgoingReferences.Sort( delegate( ExternalReference a, ExternalReference b )
					{
						return a.TargetAddress.CompareTo( b.TargetAddress );
					} );
				}
			}
			foreach( MethodBody method in this.Methods )
			{
				if( method.IncomingReferences.Count > 0 )
				{
					method.IncomingReferences.Sort( delegate( ExternalReference a, ExternalReference b )
					{
						return a.SourceAddress.CompareTo( b.SourceAddress );
					} );
				}
				if( ( method.IncomingReferences.Count > 0 ) || ( method.OutgoingReferences.Count > 0 ) )
					method.FinalizeReferences();
			}
		}

		private MethodBody BuildMethodBody( Method method )
		{
			Debug.Assert( this.Debugger.DebugHost.CpuHook != null );
			uint[] codes = this.Debugger.DebugHost.CpuHook.GetMethodBody( method );

			uint instrAddress = method.Address;
			List<Instruction> instrs = new List<Instruction>( ( int )method.Length / 4 );
			for( int n = 0; n < codes.Length; n++ )
			{
				Instruction instr = new Instruction( instrAddress, codes[ n ] );
				instrs.Add( instr );
				instrAddress += 4;
			}
			MethodBody methodBody = new MethodBody( method.Address, ( uint )method.Length, instrs.ToArray() );
			if( method.Type == MethodType.Bios )
			{
				if( method.Function.MethodName != null )
					methodBody.Name = method.Function.MethodName;
				else
					methodBody.Name = method.Function.ModuleName + "::" + method.Function.NID.ToString( "X8" );
				methodBody.Function = method.Function;
			}
			else
			{
				if( method.Name != null )
					methodBody.Name = method.Name;
			}

			return methodBody;
		}

		public MethodBody this[ uint address ]
		{
			get
			{
				int first = 0;
				int last = this.Methods.Count - 1;
				while( first <= last )
				{
					int middle = ( first + last ) / 2;
					MethodBody body = this.Methods[ middle ];
					if( ( address >= body.Address ) &&
						( address < body.Address + body.Length ) )
						return body;
					else if( body.Address < address )
						first = middle + 1;
					else
						last = middle - 1;
				}
				return null;
			}
		}
	}
}
