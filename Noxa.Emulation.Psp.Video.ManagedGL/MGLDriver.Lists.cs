// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Noxa.Emulation.Psp.Video.ManagedGL
{
	unsafe partial class MGLDriver
	{
		private const int DisplayListCount = 16;
		private readonly DisplayList[] _displayLists = new DisplayList[ DisplayListCount ];
		private int _displayListIndex = 0;

		private bool _isPaused;
		private bool _hasFinished;

		private void SetupLists()
		{
			for( int n = 0; n < _displayLists.Length; n++ )
			{
				_displayLists[ n ] = new DisplayList();
				_displayLists[ n ].ID = n;
				_displayLists[ n ].State = DisplayListState.Done;
			}
		}

		public int EnqueueList( uint listAddress, uint stallAddress, int callbackId, uint contextAddress, bool insertAtHead )
		{
			_hasFinished = false;

			_displayListIndex++;
			if( _displayListIndex >= DisplayListCount )
				_displayListIndex = 0;
			DisplayList list = _displayLists[ _displayListIndex ];
			Debug.Assert( list.State == DisplayListState.Done );
			list.State = DisplayListState.Stalled;

			list.Pointer = ( uint* )this.MemorySystem.Translate( listAddress );
			list.StartAddress = listAddress;
			list.StallAddress = stallAddress;
			list.Base = 0x08000000;
			list.StackIndex = 0;
			list.CallbackID = callbackId;
			list.ContextAddress = contextAddress;

			list.State = ( stallAddress == 0 ) ? DisplayListState.Ready : DisplayListState.Stalled;

			// Process only if no stall address given - if given, then wait until it's all there
			if( list.State == DisplayListState.Ready )
				this.ProcessList( list );

			return list.ID;
		}

		public int UpdateList( int listId, uint stallAddress )
		{
			//if( listId == -1 )
			//{
			//    // TODO: figure out if this is valid
			//    Log.WriteLine( Verbosity.Critical, Feature.Video, "UpdateList called with listId = -1, using last list - probably wrong" );
			//    listId = _displayListIndex;
			//    //return unchecked( ( int )0x80000100 );
			//}
			if( listId < 0 )
				return 0;

			DisplayList list = _displayLists[ listId ];
			Debug.Assert( list.State == DisplayListState.Stalled );
			if( list.State != DisplayListState.Stalled )
				return -1;

			list.StallAddress = stallAddress;
			list.State = ( stallAddress == 0 ) ? DisplayListState.Ready : DisplayListState.Stalled;

			// Process only if no stall address given - if given, then wait until it's all there
			if( list.State == DisplayListState.Ready )
				this.ProcessList( list );

			return 0;
		}

		public int SyncList( int listId, int syncType )
		{
			if( listId == -1 )
			{
				// TODO: figure out if this is valid
				Log.WriteLine( Verbosity.Critical, Feature.Video, "SyncList called with listId = -1, using last list - probably wrong" );
				listId = _displayListIndex;
				//return unchecked( ( int )0x80000100 );
			}

			bool ready = false;
			bool stalled = false;
			bool drawing = false;

			DisplayList list = _displayLists[ listId ];
			switch( list.State )
			{
				case DisplayListState.Done:
					return 0;
				case DisplayListState.Stalled:
					stalled = true;
					break;
				case DisplayListState.Ready:
					ready = true;
					break;
				case DisplayListState.Drawing:
					drawing = true;
					break;
			}

			// Force processing? We do it all inline, so we should never be drawing, only stalled
			bool block = ( syncType == 0 );
			if( ( block == true ) && ( ready || stalled || drawing ) )
			{
				// Wait until done drawing...
				this.ProcessAllLists();
				// TODO: kernel sleep thread?
			}

			if( _isPaused == true )
				return 4;
			else if( stalled == true )
				return 3;
			else if( drawing == true )
				return 2;
			else if( ready == true )
				return 1;
			else
				return 0;
		}

		public int SyncDraw( int syncType )
		{
			bool anyReady = false;
			bool anyStalled = false;
			bool anyDrawing = false;
			for( int n = 0; n < _displayLists.Length; n++ )
			{
				DisplayList list = _displayLists[ n ];
				switch( list.State )
				{
					case DisplayListState.Stalled:
						anyStalled = true;
						break;
					case DisplayListState.Ready:
						anyReady = true;
						break;
					case DisplayListState.Drawing:
						anyDrawing = true;
						break;
				}
			}

			// Force processing? We do it all inline, so we should never be drawing, only stalled
			bool block = ( syncType == 0 );
			if( ( block == true ) && ( anyReady || anyStalled || anyDrawing ) )
			{
				// Wait until done drawing...
				this.ProcessAllLists();
				// TODO: kernel sleep thread?
			}

			// If paused, return that
			if( _isPaused == true )
				return 4;
			else if( anyStalled == true )
				return 3;
			else if( anyDrawing == true )
				return 2;
			else if( anyReady == true )
				return 1;
			else
				return 0;
		}

		public void Break()
		{
			_isPaused = true;
			throw new NotImplementedException();
		}

		public void Continue()
		{
			_isPaused = false;
			// Process lists
			throw new NotImplementedException();
		}
	}
}
