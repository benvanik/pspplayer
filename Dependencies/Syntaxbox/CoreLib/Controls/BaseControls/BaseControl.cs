// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Puzzle.Drawing;

namespace Puzzle.Windows.Forms
{
	[ToolboxItem(true)]
	public class BaseControl : Control
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private BorderStyle borderStyle;

		private Color borderColor = Color.Black;
		private Container components = null;
		private bool RunOnce = true;

		public event EventHandler Load = null;


		public BaseControl()
		{
			SetStyle(ControlStyles.EnableNotifyMessage, true);
			this.BorderStyle = BorderStyle.FixedSingle;
			InitializeComponent();


		}

		[Browsable(false)]
		public Size WindowSize
		{
			get
			{
				APIRect s = new APIRect();
				NativeMethods.GetWindowRect(this.Handle, ref s);
				return new Size(s.Width, s.Height);
			}
		}

		[Category("Appearance - Borders"), Description("The border color")]
		[DefaultValue(typeof (Color), "Black")]
		public Color BorderColor
		{
			get { return borderColor; }

			set
			{
				borderColor = value;
				this.Refresh();
				this.Invalidate();
				UpdateStyles();
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private const int WS_EX_CLIENTEDGE = unchecked((int) 0x00000200);
		private const int WS_BORDER = unchecked((int) 0x00800000);

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;

				if (BorderStyle == BorderStyle.None)
					return cp;

				cp.ExStyle &= (~WS_EX_CLIENTEDGE);
				cp.Style &= (~WS_BORDER);

				return cp;
			}
		}

		[Browsable(true),
			EditorBrowsable(EditorBrowsableState.Always)]
		[Category("Appearance - Borders"), Description("The border style")]
		public BorderStyle BorderStyle
		{
			get { return borderStyle; }
			set
			{
				if (borderStyle != value)
				{
					if (!Enum.IsDefined(typeof (BorderStyle), value))
					{
						throw new InvalidEnumArgumentException("value", (int) value, typeof (BorderStyle));
					}
					borderStyle = value;
					UpdateStyles();
					this.Refresh();
				}
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Do not use!", true)]
		public override Image BackgroundImage
		{
			get { return base.BackgroundImage; }
			set { base.BackgroundImage = value; }
		}


		[Browsable(false)]
		public int ClientWidth
		{
			get { return this.WindowSize.Width - (this.BorderWidth*2); }
		}

		[Browsable(false)]
		public int ClientHeight
		{
			get { return this.WindowSize.Height - (this.BorderWidth*2); }
		}

		[Browsable(false)]
		public int BorderWidth
		{
			get
			{
				switch (this.borderStyle)
				{
					case BorderStyle.None:
						{
							return 0;
						}
					case BorderStyle.Sunken:
						{
							return 2;
						}
					case BorderStyle.SunkenThin:
						{
							return 1;
						}
					case BorderStyle.Raised:
						{
							return 2;
						}

					case BorderStyle.Etched:
						{
							return 2;
						}
					case BorderStyle.Bump:
						{
							return 6;
						}
					case BorderStyle.FixedSingle:
						{
							return 1;
						}
					case BorderStyle.FixedDouble:
						{
							return 2;
						}
					case BorderStyle.RaisedThin:
						{
							return 1;
						}
					case BorderStyle.Dotted:
						{
							return 1;
						}
					case BorderStyle.Dashed:
						{
							return 1;
						}
				}


				return this.Height;
			}
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// BaseControl
			// 
			this.Size = new System.Drawing.Size(272, 264);


		}

		#endregion

		protected virtual void OnLoad(EventArgs e)
		{
			if (Load != null)
				Load(this, e);
			this.Refresh();
		}

		protected override unsafe void WndProc(ref Message m)
		{
			if (m.Msg == (int) WindowMessage.WM_NCPAINT)
			{
				RenderBorder();
			}
			else if (m.Msg == (int) WindowMessage.WM_SHOWWINDOW)
			{
				if (RunOnce)
				{
					RunOnce = false;
					OnLoad(null);
					base.WndProc(ref m);
					UpdateStyles();
				}
				else
				{
					UpdateStyles();
					base.WndProc(ref m);
				}

			}
			else if (m.Msg == (int) WindowMessage.WM_NCCREATE)
			{
				base.WndProc(ref m);
			}
			else if (m.Msg == (int) WindowMessage.WM_NCCALCSIZE)
			{
				if (m.WParam == (IntPtr) 0)
				{
					APIRect* pRC = (APIRect*) m.LParam;
					//pRC->left -=3;
					base.WndProc(ref m);
				}
				else if (m.WParam == (IntPtr) 1)
				{
					_NCCALCSIZE_PARAMS* pNCP = (_NCCALCSIZE_PARAMS*) m.LParam;


					int t = pNCP->NewRect.top + this.BorderWidth;
					int l = pNCP->NewRect.left + this.BorderWidth;
					int b = pNCP->NewRect.bottom - this.BorderWidth;
					int r = pNCP->NewRect.right - this.BorderWidth;

					base.WndProc(ref m);

					pNCP->NewRect.top = t;
					pNCP->NewRect.left = l;
					pNCP->NewRect.right = r;
					pNCP->NewRect.bottom = b;

					return;
				}
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		private void RenderBorder()
		{
			IntPtr hdc = NativeMethods.GetWindowDC(this.Handle);
			APIRect s = new APIRect();
			NativeMethods.GetWindowRect(this.Handle, ref s);

			using (Graphics g = Graphics.FromHdc(hdc))
			{
				DrawingTools.DrawBorder((BorderStyle2) (int) this.BorderStyle, this.BorderColor, g, new Rectangle(0, 0, s.Width, s.Height));
			}
			NativeMethods.ReleaseDC(this.Handle, hdc);
		}


		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
		}

//		protected override void OnHandleCreated(System.EventArgs e)
//		{
//			base.OnHandleCreated (e);
//		//	this.UpdateStyles ();
//			Console.WriteLine ("gapa");
//		}
//
//		protected override void OnHandleDestroyed(System.EventArgs e)
//		{			
//			base.OnHandleDestroyed (e);
//			Console.WriteLine ("apa");
//		}
//
//		protected override void OnParentChanged(System.EventArgs e)
//		{
//			base.OnParentChanged (e);
//		}
	}
}