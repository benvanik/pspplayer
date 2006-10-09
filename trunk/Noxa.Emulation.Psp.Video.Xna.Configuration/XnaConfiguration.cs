// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Emulation.Psp.Video.Xna.Configuration
{
	partial class XnaConfiguration : Noxa.Emulation.Psp.Configuration.ConfigurationBase
	{
		public XnaConfiguration()
		{
			InitializeComponent();
		}

		public XnaConfiguration( ComponentParameters parameters )
			: base( parameters )
		{
			InitializeComponent();

			this.Fill();
		}

		protected override void Fill()
		{
			if( Environment.ProcessorCount == 1 )
			{
				this.multithreadedCheckBox.Enabled = false;
				this.multithreadedCheckBox.Checked = false;
			}
			else
				this.multithreadedCheckBox.Checked = ( bool )_params[ XnaSettings.Multithreaded ];
		}

		public override ComponentParameters Save()
		{
			ComponentParameters p = _params.Clone();
			p[ XnaSettings.Multithreaded ] = this.multithreadedCheckBox.Checked;
			return p;
		}
	}
}

