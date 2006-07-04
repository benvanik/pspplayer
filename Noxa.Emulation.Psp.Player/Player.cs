using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Noxa.Emulation.Psp.Player.Configuration;

namespace Noxa.Emulation.Psp.Player
{
	partial class Player : Form
	{
		protected Host _host;

		public Player()
		{
			InitializeComponent();
		}

		public Player( Host host )
			: this()
		{
			_host = host;

			this.Icon = Properties.Resources.PspIcon;
		}

		public IntPtr ControlHandle
		{
			get
			{
				return renderSurface.Handle;
			}
		}

		private void Player_Load( object sender, EventArgs e )
		{
			CurrentInstance_StateChanged( null, EventArgs.Empty );
		}

		private delegate void DummyDelegate();
		void CurrentInstance_StateChanged( object sender, EventArgs e )
		{
			DummyDelegate del = delegate()
			{
				InstanceState state;
				if( _host.CurrentInstance != null )
					state = _host.CurrentInstance.State;
				else
					state = InstanceState.Idle;

				this.pauseToolStripButton.Checked = ( state == InstanceState.Paused );

				bool invalidate = false;
				switch( state )
				{
					case InstanceState.Idle:
						this.startToolStripButton.Enabled = true;
						this.pauseToolStripButton.Enabled = false;
						this.stopToolStripButton.Enabled = false;
						this.restartToolStripButton.Enabled = false;
						this.configureToolStripButton.Enabled = true;
						this.splashPicture.Visible = true;
						invalidate = true;
						break;
					case InstanceState.Running:
						this.startToolStripButton.Enabled = false;
						this.pauseToolStripButton.Enabled = true;
						this.stopToolStripButton.Enabled = true;
						this.restartToolStripButton.Enabled = true;
						this.configureToolStripButton.Enabled = false;
						this.splashPicture.Visible = false;
						break;
					case InstanceState.Paused:
						this.startToolStripButton.Enabled = false;
						this.pauseToolStripButton.Enabled = true;
						this.stopToolStripButton.Enabled = true;
						this.restartToolStripButton.Enabled = true;
						this.configureToolStripButton.Enabled = false;
						this.splashPicture.Visible = false;
						break;
					case InstanceState.Ended:
						this.startToolStripButton.Enabled = true;
						this.pauseToolStripButton.Enabled = false;
						this.stopToolStripButton.Enabled = false;
						this.restartToolStripButton.Enabled = true;
						this.configureToolStripButton.Enabled = true;
						this.splashPicture.Visible = true;
						invalidate = true;
						break;
					case InstanceState.Crashed:
					case InstanceState.Debugging:
					default:
						this.startToolStripButton.Enabled = false;
						this.pauseToolStripButton.Enabled = false;
						this.stopToolStripButton.Enabled = false;
						this.restartToolStripButton.Enabled = true;
						this.configureToolStripButton.Enabled = true;
						this.splashPicture.Visible = true;
						invalidate = true;
						break;
				}

				if( invalidate == true )
				{
					this.BackColor = Color.White;
					this.Invalidate( true );
					this.Update();
				}
			};

			this.Invoke( del );
		}

		private void startToolStripButton_Click( object sender, EventArgs e )
		{
			if( _host.CurrentInstance != null )
			{
				_host.CurrentInstance.StateChanged -= new EventHandler( CurrentInstance_StateChanged );
			}

			_host.CreateInstance();
			_host.CurrentInstance.StateChanged += new EventHandler( CurrentInstance_StateChanged );

			CurrentInstance_StateChanged( _host.CurrentInstance, EventArgs.Empty );

			_host.CurrentInstance.Start();
		}

		private void pauseToolStripButton_Click( object sender, EventArgs e )
		{
			if( _host.CurrentInstance.State == InstanceState.Paused )
				_host.CurrentInstance.Resume();
			else
				_host.CurrentInstance.Pause();
		}

		private void stopToolStripButton_Click( object sender, EventArgs e )
		{
			_host.CurrentInstance.Stop();
		}

		private void restartToolStripButton_Click( object sender, EventArgs e )
		{
			_host.CurrentInstance.Restart();
		}

		private void configureToolStripButton_Click( object sender, EventArgs e )
		{
			Options options = new Options( _host.ComponentSettings );
			if( options.ShowDialog( this ) != DialogResult.Cancel )
			{
				// Changes
				_host.ComponentSettings = options.ComponentSettings;
				_host.Save();
			}
		}
	}
}