// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Emulation.Psp.HookerPC
{
	partial class MainDisplay : Form
	{
		public HookerClient Client = new HookerClient();

		// For state tracking
		private uint _lastCallThreadId;
		private uint _lastCallNid;
		private uint _lastCallAddress;

		public MainDisplay()
		{
			InitializeComponent();

			this.Client.CallHit += new CallEventHandler( Client_CallHit );
			this.Client.ReturnHit += new ReturnEventHandler( Client_ReturnHit );
		}

		private void Client_CallHit( uint threadId, uint nid, uint callingAddress, string[] parameterValues )
		{
			_lastCallThreadId = threadId;
			_lastCallNid = nid;
			_lastCallAddress = callingAddress;

			// Do something
		}

		private void Client_ReturnHit( uint threadId, uint nid, uint callingAddress, string returnValue )
		{
			if( ( _lastCallThreadId == threadId ) && ( _lastCallNid == nid ) && ( _lastCallAddress == callingAddress ) )
			{
				// Return for previous call
			}
			else
			{
				// Delayed return
			}
		}

		private void connectToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Client.Connect();
		}

		private void disconnectToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Client.Disconnect();
		}

		private void clearToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.callListView.Items.Clear();
			this.stdoutListView.Items.Clear();
		}
	}
}