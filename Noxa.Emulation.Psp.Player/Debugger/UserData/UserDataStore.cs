// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Noxa.Emulation.Psp.Player.Debugger.UserData
{
	public class UserDataStore
	{
		private JsonSerializer _serializer = new JsonSerializer();
		private string _path;
		public BreakpointStore Breakpoints = new BreakpointStore();
		public CodeTagStore CodeTags = new CodeTagStore();
		public BookmarkStore Bookmarks = new BookmarkStore();

		public void Setup( string path )
		{
			_path = path;
		}

		public void Load()
		{
			if( File.Exists( _path ) == false )
				return;
			using( TextReader textReader = File.OpenText( _path ) )
			using( JsonReader reader = new JsonReader( textReader ) )
			{
				PrimaryStore store = ( PrimaryStore )_serializer.Deserialize( reader, typeof( PrimaryStore ) );
				this.Breakpoints = store.Breakpoints;
				this.CodeTags = store.CodeTags;
				this.Bookmarks = store.Bookmarks;
			}
		}

		public void Save()
		{
			using( TextWriter textWriter = new StreamWriter( _path ) )
			using( JsonWriter writer = new JsonWriter( textWriter ) )
			{
				PrimaryStore store = new PrimaryStore();
				store.Breakpoints = this.Breakpoints;
				store.CodeTags = this.CodeTags;
				store.Bookmarks = this.Bookmarks;
				_serializer.Serialize( writer, store );
			}
		}
	}

	public class PrimaryStore
	{
		public BreakpointStore Breakpoints;
		public CodeTagStore CodeTags;
		public BookmarkStore Bookmarks;
	}
}
