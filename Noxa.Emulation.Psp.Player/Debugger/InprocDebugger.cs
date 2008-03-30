// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Debugging.DebugModel;
using Noxa.Emulation.Psp.Debugging.Protocol;
using Noxa.Emulation.Psp.Games;
using Noxa.Emulation.Psp.Player.Debugger.Dialogs;
using Noxa.Emulation.Psp.Player.Debugger.Model;
using Noxa.Emulation.Psp.Player.Debugger.Tools;
using Noxa.Emulation.Psp.Player.Debugger.UserData;

namespace Noxa.Emulation.Psp.Player.Debugger
{
	partial class InprocDebugger : IDebugger, IDebugHandler
	{
		public readonly Host Host;
		public readonly DebugHost DebugHost;
		public readonly DebuggerWindow Window;
		public readonly List<DebuggerTool> Tools;

		public readonly BreakpointsTool BreakpointsTool;
		public readonly CallstackTool CallstackTool;
		public readonly CodeTool CodeTool;
		public readonly HandlesTool HandlesTool;
		public readonly LogTool LogTool;
		public readonly MemoryTool MemoryTool;
		public readonly ModulesTool ModulesTool;
		public readonly StatisticsTool StatisticsTool;
		public readonly StringsTool StringsTool;
		public readonly SyscallsTool SyscallsTool;
		public readonly ThreadsTool ThreadsTool;
		public readonly WatchTool WatchTool;

		public DebuggerState State;
		public uint PC;
		public BreakpointManager Breakpoints;
		public CodeCache CodeCache;
		public UserDataStore UserData;

		public InprocDebugger( Host host )
		{
			this.Host = host;
			this.DebugHost = host.Debugger;
			this.Window = new DebuggerWindow( this );
			this.Tools = new List<DebuggerTool>();

			this.State = DebuggerState.Idle;
			this.Breakpoints = new BreakpointManager( this );
			this.CodeCache = new CodeCache( this );
			this.UserData = new UserDataStore();

			this.SetupNavigation();

			// Initialize tools...
			// ...
			this.CallstackTool = new CallstackTool( this );
			this.Tools.Add( this.CallstackTool );
			this.CodeTool = new CodeTool( this );
			this.Tools.Add( this.CodeTool );
			this.LogTool = new LogTool( this );
			this.Tools.Add( this.LogTool );
			this.MemoryTool = new MemoryTool( this );
			this.Tools.Add( this.MemoryTool );
			this.StatisticsTool = new StatisticsTool( this );
			this.Tools.Add( this.StatisticsTool );
			this.ThreadsTool = new ThreadsTool( this );
			this.Tools.Add( this.ThreadsTool );
			// ...

			this.Window.Show();

			this.CodeTool.Show( this.Window.DockPanel );
			this.LogTool.Show( this.Window.DockPanel );
			this.ThreadsTool.Show( this.Window.DockPanel );

			WeifenLuo.WinFormsUI.Docking.DockPane dp;
			dp = this.Window.DockPanel.DockPaneFactory.CreateDockPane( this.CodeTool, WeifenLuo.WinFormsUI.Docking.DockState.Document, true );
			this.StatisticsTool.Show( dp, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.45 );
			dp = this.Window.DockPanel.DockPaneFactory.CreateDockPane( this.LogTool, WeifenLuo.WinFormsUI.Docking.DockState.DockBottom, true );
			this.CallstackTool.Show( dp, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.24 );

			this.MemoryTool.Show( this.StatisticsTool.DockHandler.Pane, this.StatisticsTool );

			this.Host.Debugger.Activate( this, Environment.MachineName, Environment.UserName, "InprocDebugger 1.0" );

			foreach( DebuggerTool tool in this.Tools )
				tool.OnAttached();
		}

		#region IDebugger Members

		public ILogger Logger { get { return this.LogTool; } }
		public IDebugHandler Handler { get { return this; } }

		private int _lastId = 100;
		public int AllocateID()
		{
			return Interlocked.Increment( ref _lastId );
		}

		public void OnStarted( GameInformation game, Stream bootStream )
		{
			// Need to marshal all UI calls to the proper thread
			DummyDelegate del = delegate
			{
				this.State = DebuggerState.Running;
				this.OnStateChanged();

				foreach( DebuggerTool tool in this.Tools )
					tool.OnStarted();
			};
			this.Window.Invoke( del );
		}

		public void OnStopped()
		{
			DummyDelegate del = delegate
			{
				this.State = DebuggerState.Detached;
				this.OnStateChanged();
			};
			this.Window.Invoke( del );
		}

		public void OnBootModuleLoaded( uint entryAddress )
		{
			string dataPath = "Data-" + this.DebugHost.Emulator.CurrentInstance.Bios.Game.Parameters.DiscID + ".ddb";
			this.UserData.Setup( dataPath );
			this.UserData.Load();
			this.Breakpoints.Load();

			//Breakpoint startupBp = new Breakpoint( this.AllocateID(), BreakpointType.CodeExecute, entryAddress );
			//this.Breakpoints.Add( startupBp );

			// TEST
			//Breakpoint bp2 = new Breakpoint( this.AllocateID(), 0x09FFFEF4, Noxa.Emulation.Psp.Debugging.Hooks.MemoryAccessType.ReadWrite );
			//this.Breakpoints.Add( bp2 );
		}

		public void OnModuleLoaded()
		{
			DummyDelegate del = delegate
			{
				try
				{
					this.CodeCache.Update();
					this.Breakpoints.Update();
					this.CodeTool.InvalidateAll();
				}
				catch
				{
					throw;
				}
			};
			this.Window.Invoke( del );
		}

		#endregion

		public void BringToFront()
		{
			// Needs invoke?
			this.Window.BringToFront();
		}

		public event EventHandler StateChanged;
		private void OnStateChanged()
		{
			//DummyDelegate del = delegate
			//{
			//};
			//this.Window.Invoke( del );
			EventHandler handler = this.StateChanged;
			if( handler != null )
				handler( this, EventArgs.Empty );
		}

		public void SetStatusText( string format, params object[] args )
		{
			string final = ( args.Length == 0 ) ? format : string.Format( format, args );
			this.Window.SetStatusText( final );
		}
	}
}
