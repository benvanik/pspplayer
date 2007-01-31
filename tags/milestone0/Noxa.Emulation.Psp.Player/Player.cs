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
using Noxa.Emulation.Psp.Player.Configuration;
using Noxa.Emulation.Psp.Player.Development;

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
		protected int _widthPadding;
		protected int _heightPadding;

		public Player()
		{
			InitializeComponent();
		}

		public Player( Host host )
			: this()
		{
			_host = host;

			this.Icon = Properties.Resources.PspIcon;

			this.SetupGlass();

			_widthPadding = this.Width - 480;
			_heightPadding = this.Height - 272;
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
						this.debugToolStripButton.Enabled = true;
						this.attachToolStripButton.Enabled = false;
						this.splashPicture.Visible = true;
						invalidate = true;
						break;
					case InstanceState.Running:
						this.startToolStripButton.Enabled = false;
						this.pauseToolStripButton.Enabled = true;
						this.stopToolStripButton.Enabled = true;
						this.restartToolStripButton.Enabled = true;
						this.configureToolStripButton.Enabled = false;
						this.debugToolStripButton.Enabled = false;
						this.attachToolStripButton.Enabled = true;
						this.splashPicture.Visible = false;
						break;
					case InstanceState.Paused:
						this.startToolStripButton.Enabled = false;
						this.pauseToolStripButton.Enabled = true;
						this.stopToolStripButton.Enabled = true;
						this.restartToolStripButton.Enabled = true;
						this.configureToolStripButton.Enabled = false;
						this.debugToolStripButton.Enabled = false;
						this.attachToolStripButton.Enabled = true;
						this.splashPicture.Visible = false;
						break;
					case InstanceState.Ended:
						this.startToolStripButton.Enabled = true;
						this.pauseToolStripButton.Enabled = false;
						this.stopToolStripButton.Enabled = false;
						this.restartToolStripButton.Enabled = true;
						this.configureToolStripButton.Enabled = true;
						this.debugToolStripButton.Enabled = true;
						this.attachToolStripButton.Enabled = false;
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
						this.debugToolStripButton.Enabled = false;
						this.attachToolStripButton.Enabled = true;
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

		private void StartInstance( bool debugging )
		{
			if( _host.CurrentInstance != null )
			{
				_host.CurrentInstance.StateChanged -= new EventHandler( CurrentInstance_StateChanged );
			}

			if( _host.CreateInstance() == false )
				return;

			_host.CurrentInstance.StateChanged += new EventHandler( CurrentInstance_StateChanged );

			CurrentInstance_StateChanged( _host.CurrentInstance, EventArgs.Empty );

			_host.CurrentInstance.Start( debugging );
		}

		private void startToolStripButton_Click( object sender, EventArgs e )
		{
			this.StartInstance( false );
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

		private void debugToolStripButton_Click( object sender, EventArgs e )
		{
			this.StartInstance( true );
		}

		private void attachToolStripButton_Click( object sender, EventArgs e )
		{
			if( _host.Debugger != null )
			{
				_host.Debugger.Show();
				return;
			}

			_host.AttachDebugger();
		}

		private string GetStatusText()
		{
			InstanceState state;
			if( _host.CurrentInstance != null )
				state = _host.CurrentInstance.State;
			else
				state = InstanceState.Idle;

			switch( state )
			{
				case InstanceState.Idle:
					return "Idle";
				case InstanceState.Paused:
					return "Paused - press play to resume";
				case InstanceState.Running:
					{
						if( _host.CurrentInstance.Bios.Kernel.Game != null )
						{
							string status = string.Format( "Running {0}",
								_host.CurrentInstance.Bios.Kernel.Game.Parameters.Title );
							if( ( _host.CurrentInstance.Cpu.Capabilities.SupportedStatistics & Noxa.Emulation.Psp.Cpu.CpuStatisticsCapabilities.InstructionsPerSecond ) != 0 )
							{
								string ips = string.Format( "IPS: {0:###.##}M",
									_host.CurrentInstance.Cpu.Statistics.InstructionsPerSecond / 1000000.0 );
								status = string.Format( "{0} - {1}", status, ips );
							}
							return status;
						}
						else
							return "Selecting game...";
					}
				case InstanceState.Ended:
					return "Execution completed successfully";
				case InstanceState.Crashed:
					return "Crashed - see dump report for more information";
				case InstanceState.Debugging:
					return "Debugging";
			}

			return "";
		}

		private void UpdateStatusText()
		{
			this.PaintMe();
		}

		private void statusUpdateTimer_Tick( object sender, EventArgs e )
		{
			this.UpdateStatusText();
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

			this.WindowState = FormWindowState.Normal;
			switch( newSize )
			{
				case DisplaySize.Original:
					this.Size = new Size( 480 + _widthPadding, 272 + _heightPadding );
					break;
				case DisplaySize.TwoX:
					this.Size = new Size( ( 480 * 2 ) + _widthPadding, ( 272 * 2 ) + _heightPadding );
					break;
				case DisplaySize.ThreeX:
					this.Size = new Size( ( 480 * 3 ) + _widthPadding, ( 272 * 3 ) + _heightPadding );
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