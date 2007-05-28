// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp.GameTester
{
	class TestHost : IEmulationHost
	{
		protected TestInstance _instance;
		protected Logger _logger;

		public TestHost()
		{
			_logger = new Logger();
		}

		public IEmulationInstance CurrentInstance
		{
			get
			{
				return _instance;
			}
		}

		public bool CreateInstance()
		{
			EmulationParameters emulationParams = new EmulationParameters();

			emulationParams.BiosComponent = new Noxa.Emulation.Psp.Bios.MHLE();
			emulationParams.CpuComponent = new Noxa.Emulation.Psp.Cpu.UltraCpu();
			//emulationParams.InputComponent = new Noxa.Emulation.Psp.Input.SimpleInput();
			emulationParams.MemoryStickComponent = new Noxa.Emulation.Psp.Media.UserHostFileSystem();
			emulationParams.UmdComponent = new Noxa.Emulation.Psp.Media.Iso.IsoFileSystem();
			//emulationParams.VideoComponent = new Noxa.Emulation.Psp.Video.OpenGLVideo();

			emulationParams[ emulationParams.MemoryStickComponent ] = new ComponentParameters();
			emulationParams[ emulationParams.MemoryStickComponent ][ "path" ] = @"C:\Dev\Noxa.Emulation\trunk\Test Stick\";

			_instance = new TestInstance( this, emulationParams );

			List<ComponentIssue> issues = _instance.Test();
			bool showReport = false;
			bool allowRun = true;
			if( issues.Count > 0 )
			{
				int errorCount = 0;
				foreach( ComponentIssue issue in issues )
				{
					if( issue.Level == IssueLevel.Error )
						errorCount++;
				}
				if( errorCount == 0 )
				{
					showReport = false;
				}
				else
				{
					showReport = true;
					allowRun = false;
				}
			}

			Debug.Assert( showReport == false );
			if( showReport == true )
			{
			}

			if( allowRun == true )
				return _instance.Create();
			else
				return false;
		}

		#region Debugging Members

		public bool IsDebuggerAttached
		{
			get
			{
				return false;
			}
		}

		public DebugHost Debugger
		{
			get
			{
				return null;
			}
		}

		public void AttachDebugger()
		{
		}

		public bool AskForDebugger( string message )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		#endregion
	}
}
