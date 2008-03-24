namespace Noxa.Emulation.Psp.Player.Debugger.Tools
{
	partial class LogTool
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.logControl = new Noxa.Emulation.Psp.Player.Debugger.Tools.LogControl();
			this.SuspendLayout();
			// 
			// logControl
			// 
			this.logControl.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.logControl.Location = new System.Drawing.Point( 4, 4 );
			this.logControl.Name = "logControl";
			this.logControl.Size = new System.Drawing.Size( 821, 174 );
			this.logControl.TabIndex = 0;
			this.logControl.Text = "logControl";
			// 
			// LogTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.ClientSize = new System.Drawing.Size( 829, 182 );
			this.CloseButton = false;
			this.Controls.Add( this.logControl );
			this.Name = "LogTool";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
			this.TabText = "Log";
			this.ResumeLayout( false );

		}

		#endregion

		private LogControl logControl;
	}
}
