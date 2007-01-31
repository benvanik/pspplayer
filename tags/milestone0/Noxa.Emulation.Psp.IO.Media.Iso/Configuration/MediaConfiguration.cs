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

namespace Noxa.Emulation.Psp.IO.Media.Iso.Configuration
{
	partial class MediaConfiguration : Noxa.Emulation.Psp.Configuration.ConfigurationBase
	{
		public const string PathSetting = "path";

		public MediaConfiguration()
		{
			InitializeComponent();
		}

		public MediaConfiguration( ComponentParameters parameters )
			: base( parameters )
		{
			InitializeComponent();

			this.Fill();
		}

		protected override void Fill()
		{
			this.pathTextBox.Text = _params[ PathSetting ] as string;
		}

		public override ComponentParameters Save()
		{
			ComponentParameters p = _params.Clone();
			p[ PathSetting ] = this.pathTextBox.Text;
			return p;
		}

		private void BrowseButtonClick( object sender, EventArgs e )
		{
			if( this.openFileDialog.ShowDialog( this ) == DialogResult.OK )
			{
				string path = this.openFileDialog.FileName;
				this.pathTextBox.Text = path;
			}
		}
	}
}

