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

namespace Noxa.Emulation.Psp.IO.Media.FileSystem.Configuration
{
	partial class MediaConfiguration : Noxa.Emulation.Psp.Configuration.ConfigurationBase
	{
		protected ComponentType _mediaType;

		public const string PathSetting = "path";
		public const string ReadOnlySetting = "readOnly";
		public const string CapacitySetting = "capacity";

		public MediaConfiguration()
		{
			InitializeComponent();
		}

		public MediaConfiguration( ComponentType mediaType, ComponentParameters parameters )
			: base( parameters )
		{
			InitializeComponent();

			_mediaType = mediaType;

			this.Fill();
		}

		protected override void Fill()
		{
			this.pathTextBox.Text = _params[ PathSetting ] as string;

			bool readOnly = _params.GetValue<bool>( ReadOnlySetting, false );
			long capacity = _params.GetValue<long>( CapacitySetting, ( long )1024 * 1024 * 1024 * 2 );
			
			this.readOnlyCheckBox.Checked = readOnly;
			this.capacityComboBox.Text = capacity.ToString();

			if( _mediaType == ComponentType.UserMedia )
				this.readOnlyCheckBox.Visible = true;
			else
				this.readOnlyCheckBox.Visible = false;
		}

		public override ComponentParameters Save()
		{
			ComponentParameters p = _params.Clone();
			p[ PathSetting ] = this.pathTextBox.Text;
			p.SetValue<bool>( ReadOnlySetting, this.readOnlyCheckBox.Checked );
			long capacity = -1;
			try
			{
				capacity = Convert.ToInt64( this.capacityComboBox.Text );
			}
			catch
			{
			}
			if( capacity > 0 )
				p.SetValue<long>( CapacitySetting, capacity );
			return p;
		}

		private void BrowseButtonClick( object sender, EventArgs e )
		{
			if( this.folderBrowserDialog.ShowDialog( this ) == DialogResult.OK )
			{
				string path = this.folderBrowserDialog.SelectedPath;
				this.pathTextBox.Text = path;
			}
		}
	}
}

