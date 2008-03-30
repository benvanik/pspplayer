// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Debugging.Hooks;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	class BiosDebugHook : IBiosHook
	{
		public readonly Bios Bios;

		public BiosDebugHook( Bios bios )
		{
			this.Bios = bios;
		}

		#region Threads

		public uint ActiveThreadID
		{
			get
			{
				Kernel kernel = this.Bios._kernel;
				if( kernel.ActiveThread == null )
					return 0;
				else
					return ( uint )kernel.ActiveThread.UID;
			}
		}

		public ThreadInfo[] GetThreads()
		{
			Kernel kernel = this.Bios._kernel;
			List<ThreadInfo> threadInfos = new List<ThreadInfo>();
			foreach( KThread thread in kernel.Threads )
			{
				ThreadInfo threadInfo = new ThreadInfo();
				threadInfo.ThreadID = thread.UID;
				threadInfo.InternalThreadID = thread.ContextID;
				threadInfo.Name = thread.Name;
				threadInfo.Attributes = ( ThreadAttributes )thread.Attributes;
				threadInfo.EntryAddress = thread.EntryAddress;
				threadInfo.Priority = thread.Priority;
				threadInfo.State = ( ThreadState )thread.State;
				threadInfo.IsWaiting = ( thread.State == KThreadState.Waiting ) || ( thread.State == KThreadState.WaitSuspended );
				if( threadInfo.IsWaiting == true )
				{
					// TODO: more descriptive...
					threadInfo.WaitingDescription = thread.WaitingOn.ToString();
				}

				threadInfo.CurrentPC = this.Bios.Emulator.Cpu.GetContextRegister( thread.ContextID, -1 );
				threadInfos.Add( threadInfo );
			}
			return threadInfos.ToArray();
		}

		public void WakeThread( uint threadId )
		{
			Kernel kernel = this.Bios._kernel;
			KThread thread = kernel.GetHandleOrNull<KThread>( ( int )threadId );
			if( thread == null )
				return;
			if( ( thread.State == KThreadState.Suspended ) ||
				( thread.State == KThreadState.WaitSuspended ) )
				thread.Resume();
			if( thread.State == KThreadState.Waiting )
				thread.ReleaseWait( false );
		}

		public void DelayThread( uint threadId, uint delayMs )
		{
			Kernel kernel = this.Bios._kernel;
			KThread thread = kernel.GetHandleOrNull<KThread>( ( int )threadId );
			if( thread == null )
				return;
			thread.Delay( delayMs * 1000, false );
		}

		public void SuspendThread( uint threadId )
		{
			Kernel kernel = this.Bios._kernel;
			KThread thread = kernel.GetHandleOrNull<KThread>( ( int )threadId );
			if( thread == null )
				return;
			thread.Suspend();
		}

		public void ResumeThread( uint threadId )
		{
			Kernel kernel = this.Bios._kernel;
			KThread thread = kernel.GetHandleOrNull<KThread>( ( int )threadId );
			if( thread == null )
				return;
			thread.Resume();
		}

		public void KillThread( uint threadId )
		{
			Kernel kernel = this.Bios._kernel;
			KThread thread = kernel.GetHandleOrNull<KThread>( ( int )threadId );
			if( thread == null )
				return;
			thread.Exit( -1 );
		}

		#endregion

		#region Modules

		public ModuleInfo[] GetModules()
		{
			Kernel kernel = this.Bios._kernel;
			List<ModuleInfo> moduleInfos = new List<ModuleInfo>();
			foreach( KModule module in kernel.UserModules )
			{
				ModuleInfo moduleInfo = new ModuleInfo();
				moduleInfo.ModuleID = module.UID;
				moduleInfo.Name = module.Name;
				moduleInfo.Path = module.LoadParameters.FilePath;
				moduleInfo.EntryAddress = module.LoadResults.EntryAddress;
				moduleInfo.LowerBounds = module.LoadResults.LowerBounds;
				moduleInfo.UpperBounds = module.LoadResults.UpperBounds;
				moduleInfos.Add( moduleInfo );
			}
			return moduleInfos.ToArray();
		}

		#endregion
	}
}
