// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Debugging.Protocol;
using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp.Player.Debugger
{
	class InprocDebugger : IDebugger, IDebugHandler
	{
		public InprocDebugger( Host host )
		{
		}

		#region IDebugger Members

		public ILogger Logger
		{
			get { throw new NotImplementedException(); }
		}

		public IDebugHandler Handler { get { return this; } }

		public void OnStarted( Noxa.Emulation.Psp.Games.GameInformation game, System.IO.Stream bootStream )
		{
			throw new NotImplementedException();
		}

		public void OnStopped()
		{
			throw new NotImplementedException();
		}

		public int AllocateID()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDebugHandler Members

		public void OnContinue( bool steppingForward )
		{
			throw new NotImplementedException();
		}

		public void OnStepComplete( uint address )
		{
			throw new NotImplementedException();
		}

		public void OnBreakpointHit( int id )
		{
			throw new NotImplementedException();
		}

		public void OnEvent( Noxa.Emulation.Psp.Debugging.DebugModel.Event biosEvent )
		{
			throw new NotImplementedException();
		}

		public bool OnError( Noxa.Emulation.Psp.Debugging.DebugModel.Error error )
		{
			throw new NotImplementedException();
		}

		#endregion

		public void BringToFront()
		{
			throw new NotImplementedException();
		}
	}
}
