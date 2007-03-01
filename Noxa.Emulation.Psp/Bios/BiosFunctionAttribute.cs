// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Bios
{
	[global::System.AttributeUsage( AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
	public sealed class BiosFunctionAttribute : Attribute
	{
		private readonly uint _nid;
		private readonly string _name;

		public BiosFunctionAttribute( uint nid, string name )
		{
			_nid = nid;
			_name = name;
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
	}
}
