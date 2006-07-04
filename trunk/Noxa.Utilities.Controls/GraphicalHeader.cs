using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Utilities.Controls
{
	public partial class GraphicalHeader : Control
	{
		public GraphicalHeader()
		{
			InitializeComponent();

			label1.Resize += new EventHandler( LabelResize );

			label1.AutoSize = false;

			label1.Left = 2;
			label1.Top = 2;
			label1.Height = this.Height - label1.Top - 2;
			label1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;

			this.Height = label1.Height;

			this.DoubleBuffered = true;
		}

		protected override Size DefaultMinimumSize
		{
			get
			{
				return new Size( 1, 22 );
			}
		}

		protected override Size DefaultMaximumSize
		{
			get
			{
				return new Size( 9999, 22 );
			}
		}

		protected override void OnPaint( PaintEventArgs pe )
		{
			// Calling the base class OnPaint
			base.OnPaint( pe );

			int right = label1.Right;
			right += 2;
			pe.Graphics.DrawLine( SystemPens.ControlDark, right, this.Height / 2, this.Width - 10, this.Height / 2 );
		}

		public override string Text
		{
			get
			{
				return label1.Text;
			}
			set
			{
				label1.Text = value;
				
				Graphics g = label1.CreateGraphics();
				SizeF size = g.MeasureString( label1.Text, label1.Font );
				label1.Width = ( int )size.Width + 5;

				label1.Invalidate();
				label1.Update();
			}
		}

		void LabelResize( object sender, EventArgs e )
		{
			this.Invalidate();
		}
	}
}
