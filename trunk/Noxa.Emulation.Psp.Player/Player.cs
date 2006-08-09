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
	enum DisplaySize
	{
		Original,
		TwoX,
		ThreeX,
		Fullscreen
	}

	partial class Player : Form
	{
		protected Host _host;
		protected DisplaySize _displaySize;

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

			DisplaySize defaultSize = ( DisplaySize )Enum.Parse( typeof( DisplaySize ), Properties.Settings.Default.PlayerDisplaySize );
			this.SwitchToDisplaySize( DisplaySize.Original );
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

		#region Display Size

		private void sizeToolStripSplitButton_ButtonClick( object sender, EventArgs e )
		{
			switch( _displaySize )
			{
				case DisplaySize.Original:
					this.SwitchToDisplaySize( DisplaySize.TwoX );
					break;
				case DisplaySize.TwoX:
					this.SwitchToDisplaySize( DisplaySize.ThreeX );
					break;
				case DisplaySize.ThreeX:
					this.SwitchToDisplaySize( DisplaySize.Fullscreen );
					break;
				case DisplaySize.Fullscreen:
					this.SwitchToDisplaySize( DisplaySize.Original );
					break;
			}
		}

		private void originalPSPDimensionsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.SwitchToDisplaySize( DisplaySize.Original );
		}

		private void twoXToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.SwitchToDisplaySize( DisplaySize.TwoX );
		}

		private void threeXToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.SwitchToDisplaySize( DisplaySize.ThreeX );
		}

		private void fullscreenToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.SwitchToDisplaySize( DisplaySize.Fullscreen );
		}

		public void SwitchToDisplaySize( DisplaySize newSize )
		{
			if( newSize == _displaySize )
				return;

			this.originalPSPDimensionsToolStripMenuItem.Checked = ( newSize == DisplaySize.Original );
			this.twoXToolStripMenuItem.Checked = ( newSize == DisplaySize.TwoX );
			this.threeXToolStripMenuItem.Checked = ( newSize == DisplaySize.ThreeX );
			this.fullscreenToolStripMenuItem.Checked = ( newSize == DisplaySize.Fullscreen );

			int widthPadding = 8;
			int heightPadding = 59;

			this.WindowState = FormWindowState.Normal;
			switch( newSize )
			{
				case DisplaySize.Original:
					this.Size = new Size( 480 + widthPadding, 272 + heightPadding );
					break;
				case DisplaySize.TwoX:
					this.Size = new Size( ( 480 * 2 ) + widthPadding, ( 272 * 2 ) + heightPadding );
					break;
				case DisplaySize.ThreeX:
					this.Size = new Size( ( 480 * 3 ) + widthPadding, ( 272 * 3 ) + heightPadding );
					break;
				case DisplaySize.Fullscreen:
					this.WindowState = FormWindowState.Maximized;
					break;
			}

			_displaySize = newSize;

			Properties.Settings.Default.PlayerDisplaySize = _displaySize.ToString();
			Properties.Settings.Default.Save();
		}

		#endregion
	}
}