// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

namespace Noxa.Emulation.Psp.Video.Xna.Configuration
{
	partial class XnaConfiguration
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
			this.graphicalHeader1 = new Noxa.Utilities.Controls.GraphicalHeader();
			this.multithreadedCheckBox = new System.Windows.Forms.CheckBox();
			this.panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel
			// 
			this.panel.Controls.Add( this.multithreadedCheckBox );
			this.panel.Controls.Add( this.graphicalHeader1 );
			this.panel.Size = new System.Drawing.Size( 387, 357 );
			// 
			// graphicalHeader1
			// 
			this.graphicalHeader1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.graphicalHeader1.Location = new System.Drawing.Point( 3, 3 );
			this.graphicalHeader1.Name = "graphicalHeader1";
			this.graphicalHeader1.Size = new System.Drawing.Size( 381, 22 );
			this.graphicalHeader1.TabIndex = 0;
			this.graphicalHeader1.Text = "General";
			// 
			// multithreadedCheckBox
			// 
			this.multithreadedCheckBox.AutoSize = true;
			this.multithreadedCheckBox.Location = new System.Drawing.Point( 28, 31 );
			this.multithreadedCheckBox.Name = "multithreadedCheckBox";
			this.multithreadedCheckBox.Size = new System.Drawing.Size( 245, 17 );
			this.multithreadedCheckBox.TabIndex = 1;
			this.multithreadedCheckBox.Text = "Use multiple threads (multi-core machines only)";
			this.multithreadedCheckBox.UseVisualStyleBackColor = true;
			// 
			// XnaConfiguration
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.Name = "XnaConfiguration";
			this.Size = new System.Drawing.Size( 387, 379 );
			this.panel.ResumeLayout( false );
			this.panel.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion

		private Noxa.Utilities.Controls.GraphicalHeader graphicalHeader1;
		private System.Windows.Forms.CheckBox multithreadedCheckBox;
	}
}
