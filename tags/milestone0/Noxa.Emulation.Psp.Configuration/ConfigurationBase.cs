// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Noxa.Emulation.Psp.Configuration
{
	public enum Notification
	{
		None,
		ReadOnly,
		RestartRequired
	}

	public partial class ConfigurationBase : UserControl, IComponentConfiguration
	{
		protected ComponentParameters _params;
		protected ComponentParameters _defaults;
		protected Notification _notification = Notification.None;

		public ConfigurationBase()
		{
			InitializeComponent();
		}

		protected ConfigurationBase( ComponentParameters parameters )
			: this()
		{
			Debug.Assert( parameters != null );
			_params = parameters;
			_defaults = parameters.Clone();
		}

		public ComponentParameters Parameters
		{
			get
			{
				return _params;
			}
		}

		public ComponentParameters DefaultParameters
		{
			get
			{
				return _params;
			}
		}

		public Notification Notification
		{
			get
			{
				return _notification;
			}
			set
			{
				if( _notification != Notification.ReadOnly )
					_notification = value;

				Image image = null;
				string label = string.Empty;
				switch( _notification )
				{
					case Notification.ReadOnly:
						image = Properties.Resources.Lock;
						label = "These parameters are locked. Please stop the emulator before attempting to change these options.";
						break;
					case Notification.RestartRequired:
						image = Properties.Resources.Warning;
						label = "Some of your changes require a restart of the emulator before they will take effect.";
						break;
				}
				this.notificationIcon.Image = image;
				this.notificationLabel.Text = label;

				bool visible = ( _notification != Notification.None );
				this.notificationIcon.Visible = visible;
				this.notificationLabel.Visible = visible;
			}
		}

		public void RestoreDefaults()
		{
			_params = _defaults.Clone();
			this.Fill();
		}

		public virtual ComponentParameters Save()
		{
			return _params.Clone();
		}

		protected virtual void Fill()
		{
		}
	}
}
