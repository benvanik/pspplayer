using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Noxa.Emulation.Psp.Player.ServiceTools
{
	public partial class PleaseWaitDialog : Form
	{
		public PleaseWaitDialog()
		{
			InitializeComponent();
		}

		public PleaseWaitDialog( string infoText, AutoResetEvent closeEvent )
			: this()
		{
			this.cancelButton.Enabled = false;

			this.infoLabel.Text = infoText;

			Thread waitThread = new Thread( new ParameterizedThreadStart( this.CloseWaitHandler ) );
			waitThread.Name = "PleaseWaitDialog close handler";
			waitThread.IsBackground = true;
			waitThread.Start( closeEvent );
		}

		private delegate void CloseDelegate();

		private void CloseWaitHandler( object parameter )
		{
			AutoResetEvent closeEvent = parameter as AutoResetEvent;
			closeEvent.WaitOne();

			this.Invoke( new CloseDelegate( this.Close ) );
		}

		private void cancelButton_Click( object sender, EventArgs e )
		{
			this.Close();
		}
	}
}