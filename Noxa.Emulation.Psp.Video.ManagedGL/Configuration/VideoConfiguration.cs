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

namespace Noxa.Emulation.Psp.Video.ManagedGL.Configuration
{
	partial class VideoConfiguration : Noxa.Emulation.Psp.Configuration.ConfigurationBase
	{
		public const string DummySetting = "dummy";

		public VideoConfiguration()
		{
			InitializeComponent();
		}

		public VideoConfiguration( ComponentParameters parameters )
			: base( parameters )
		{
			InitializeComponent();

			this.Fill();
		}

		protected override void Fill()
		{
			//this.pathTextBox.Text = _params[ DummySetting ] as string;
		}

		public override ComponentParameters Save()
		{
			ComponentParameters p = _params.Clone();
			//p[ DummySetting ] = this.pathTextBox.Text;
			//p.SetValue<bool>( ReadOnlySetting, this.readOnlyCheckBox.Checked );
			return p;
		}
	}
}

