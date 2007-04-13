namespace Noxa.Utilities.Controls
{
    partial class WhidbeyTabControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
			if( disposing == true )
				this.ClearTabs();
			base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.panelContainer = new System.Windows.Forms.Panel();
			this.SuspendLayout();
// 
// panelContainer
// 
			this.panelContainer.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.panelContainer.Location = new System.Drawing.Point( 108, 7 );
			this.panelContainer.Name = "panelContainer";
			this.panelContainer.Size = new System.Drawing.Size( 561, 415 );
			this.panelContainer.TabIndex = 0;
// 
// WhidbeyTabControl
// 
			this.Controls.Add( this.panelContainer );
			this.Name = "WhidbeyTabControl";
			this.Size = new System.Drawing.Size( 676, 429 );
			this.MouseLeave += new System.EventHandler( this.WhidbeyTabControl_MouseLeave );
			this.MouseMove += new System.Windows.Forms.MouseEventHandler( this.WhidbeyTabControl_MouseMove );
			this.MouseUp += new System.Windows.Forms.MouseEventHandler( this.WhidbeyTabControl_MouseUp );
			this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.WhidbeyTabControl_KeyDown );
			this.ResumeLayout( false );

		}

        #endregion

        private System.Windows.Forms.Panel panelContainer;
    }
}
