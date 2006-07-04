using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Noxa.Emulation.Psp.Configuration;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Player.Configuration
{
	partial class ConfigurationHost : Form
	{
		protected IComponent _component;
		protected ConfigurationBase _panel;

		protected ComponentParameters _result;

		public ConfigurationHost()
		{
			InitializeComponent();
		}

		public ConfigurationHost( IComponent component, ConfigurationBase panel )
			: this()
		{
			Debug.Assert( component != null );
			Debug.Assert( panel != null );

			_component = component;
			_panel = panel;

			_result = panel.Parameters;

			int xpad = this.ClientSize.Width - containerPanel.Size.Width;
			int ypad = this.ClientSize.Height - containerPanel.Size.Height;
			this.ClientSize = new Size(
				xpad + _panel.Size.Width,
				ypad + _panel.Size.Height );

			containerPanel.Controls.Add( _panel );
			_panel.Dock = DockStyle.Fill;

			okButton.Focus();

			this.Text = string.Format( "{0} Configuration", _component.Name );
		}

		public ComponentParameters Parameters
		{
			get
			{
				return _result;
			}
		}

		private void okButton_Click( object sender, EventArgs e )
		{
			_result = _panel.Save();

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void defaultsButton_Click( object sender, EventArgs e )
		{
			_panel.RestoreDefaults();
		}
	}
}