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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Puzzle.Windows.Forms.CoreLib
{
	public class SplitViewParentControl : BaseControl
	{
		protected SplitViewControl splitView;
		protected SplitViewChildControl UpperLeft;
		protected SplitViewChildControl UpperRight;
		protected SplitViewChildControl LowerLeft;
		protected SplitViewChildControl LowerRight;
		public bool DisableScrollBars = false;

		#region Private Properties

		private ArrayList _Views = null;

		protected ArrayList Views
		{
			get { return _Views; }
			set { _Views = value; }
		}

		#endregion

		private void InitializeComponent()
		{
		}

		#region roger generated code

		private void InitializeComponentInternal()
		{
			this.splitView = new SplitViewControl();
			this.SuspendLayout();
			// 
			// splitView
			// 
			this.splitView.BackColor = Color.Empty;
			this.splitView.Dock = DockStyle.Fill;
			this.splitView.LowerLeft = null;
			this.splitView.LowerRight = null;
			this.splitView.Name = "splitView";
			this.splitView.Size = new Size(248, 216);
			this.splitView.SplitviewH = -4;
			this.splitView.SplitviewV = -4;
			this.splitView.TabIndex = 0;
			this.splitView.Text = "splitView";
			this.splitView.UpperLeft = null;
			this.splitView.UpperRight = null;
			// 
			// SplitViewParentControl
			// 
			this.Controls.AddRange(new Control[]
				{
					this.splitView
				});
			this.Name = "SplitViewParentControl";
			this.Size = new Size(248, 216);
			this.ResumeLayout(false);

		}

		#endregion

		public SplitViewParentControl() : base()
		{
			this.OnCreate();

			this.InitializeComponent();
			this.InitializeComponentInternal();
			this.splitView.Resizing += new EventHandler(this.SplitView_Resizing);
			this.splitView.HideLeft += new EventHandler(this.SplitView_HideLeft);
			this.splitView.HideTop += new EventHandler(this.SplitView_HideTop);


			this.LowerRight = GetNewView();
			this.LowerRight.AllowDrop = true;
			this.LowerRight.BorderColor = Color.White;
			this.LowerRight.BorderStyle = BorderStyle.None;
			this.LowerRight.Location = new Point(0, 0);
			this.LowerRight.Size = new Size(100, 100);

			this.Views = new ArrayList();
			LowerRight.TopThumb.MouseDown += new MouseEventHandler(TopThumb_MouseDown);
			LowerRight.LeftThumb.MouseDown += new MouseEventHandler(LeftThumb_MouseDown);
			Views.Add(LowerRight);
			LowerRight.TopThumbVisible = true;
			LowerRight.LeftThumbVisible = true;
			this.splitView.Controls.Add(this.LowerRight);
			this.splitView.LowerRight = this.LowerRight;

			this.SplitView = true;
			this.ScrollBars = ScrollBars.Both;
			this.BorderStyle = BorderStyle.None;
			this.ChildBorderColor = SystemColors.ControlDark;
			this.ChildBorderStyle = BorderStyle.FixedSingle;
			this.BackColor = SystemColors.Window;
			this.Size = new Size(100, 100);
			_ActiveView = LowerRight;
		}

		/// <summary>
		/// Resets the Splitview.
		/// </summary>
		public void ResetSplitview()
		{
			this.splitView.ResetSplitview();
		}

		private void SplitView_Resizing(object sender, EventArgs e)
		{
			LowerRight.TopThumbVisible = false;
			LowerRight.LeftThumbVisible = false;
		}

		private void SplitView_HideTop(object sender, EventArgs e)
		{
			LowerRight.TopThumbVisible = true;
		}

		private void SplitView_HideLeft(object sender, EventArgs e)
		{
			LowerRight.LeftThumbVisible = true;
		}

		protected virtual void ActivateSplits()
		{
			if (this.UpperLeft == null)
			{
				this.UpperLeft = GetNewView();
				this.UpperRight = GetNewView();
				this.LowerLeft = GetNewView();

				this.splitView.Controls.AddRange(new Control[]
					{
						this.UpperLeft,
						this.LowerLeft,
						this.UpperRight
					});

				this.splitView.UpperRight = this.LowerLeft;
				this.splitView.UpperLeft = this.UpperLeft;
				this.splitView.LowerLeft = this.UpperRight;

				CreateViews();


			}
		}


		private long _ticks = 0; //splitter doubleclick timer
		protected void TopThumb_MouseDown(object sender, MouseEventArgs e)
		{
			this.ActivateSplits();

			long t = DateTime.Now.Ticks - _ticks;
			_ticks = DateTime.Now.Ticks;


			if (t < 3000000)
			{
				splitView.Split5050h();
			}
			else
			{
				splitView.InvokeMouseDownh();
			}
		}

		protected void LeftThumb_MouseDown(object sender, MouseEventArgs e)
		{
			this.ActivateSplits();

			long t = DateTime.Now.Ticks - _ticks;
			_ticks = DateTime.Now.Ticks;


			if (t < 3000000)
			{
				splitView.Split5050v();
			}
			else
			{
				splitView.InvokeMouseDownv();
			}
		}

		protected virtual void OnCreate()
		{
		}

		protected bool DoOnce = false;

		protected virtual void CreateViews()
		{
			if (UpperRight != null)
			{
				Views.Add(UpperRight);
				Views.Add(UpperLeft);
				Views.Add(LowerLeft);
			}
		}

		protected virtual SplitViewChildControl GetNewView()
		{
			return null;
		}

		protected SplitViewChildControl _ActiveView = null;

		/// <summary>
		/// Gets or Sets the active view
		/// </summary>
		[Browsable(false)]
		public ActiveView ActiveView
		{
			get
			{
				if (_ActiveView == UpperLeft)
					return ActiveView.TopLeft;

				if (_ActiveView == UpperRight)
					return ActiveView.TopRight;

				if (_ActiveView == LowerLeft)
					return ActiveView.BottomLeft;

				if (_ActiveView == LowerRight)
					return ActiveView.BottomRight;

				return (ActiveView) 0;
			}
			set
			{
				if (value != ActiveView.BottomRight)
				{
					ActivateSplits();
				}


				if (value == ActiveView.TopLeft)
					_ActiveView = UpperLeft;

				if (value == ActiveView.TopRight)
					_ActiveView = UpperRight;

				if (value == ActiveView.BottomLeft)
					_ActiveView = LowerLeft;

				if (value == ActiveView.BottomRight)
					_ActiveView = LowerRight;
			}
		}

		protected void View_Enter(object sender, EventArgs e)
		{
			_ActiveView = (SplitViewChildControl) sender;
		}

		protected void View_Leave(object sender, EventArgs e)
		{
			//	((EditViewControl)sender).RemoveFocus ();
		}

		#region PUBLIC PROPERTY SPLITVIEWV

		[Browsable(false)]
		public int SplitviewV
		{
			get { return this.splitView.SplitviewV; }
			set
			{
				if (this.splitView == null)
					return;

				this.splitView.SplitviewV = value;
			}
		}

		#endregion

		#region PUBLIC PROPERTY SPLITVIEWH

		[Browsable(false)]
		public int SplitviewH
		{
			get { return this.splitView.SplitviewH; }
			set
			{
				if (this.splitView == null)
					return;
				this.splitView.SplitviewH = value;
			}
		}

		#endregion

		#region public property ScrollBars

		private ScrollBars _ScrollBars;

		[Category("Appearance"),
			Description("Determines what Scrollbars should be visible")]
		[DefaultValue(ScrollBars.Both)]
		public ScrollBars ScrollBars
		{
			get { return _ScrollBars; }

			set
			{
				if (_Views == null)
					return;

				if (DisableScrollBars)
					value = ScrollBars.None;

				foreach (SplitViewChildControl evc in _Views)
				{
					evc.ScrollBars = value;
				}
				_ScrollBars = value;
			}
		}

		#endregion 

		#region public property SplitView

		//member variable
		private bool _SplitView;

		[Category("Appearance"),
			Description("Determines if the controls should use splitviews")]
		[DefaultValue(true)]
		public bool SplitView
		{
			get { return _SplitView; }

			set
			{
				_SplitView = value;

				if (this.splitView == null)
					return;

				if (!SplitView)
				{
					this.splitView.Visible = false;
					this.Controls.Add(LowerRight);
					LowerRight.HideThumbs();
					LowerRight.Dock = DockStyle.Fill;
				}
				else
				{
					this.splitView.Visible = true;
					this.splitView.LowerRight = LowerRight;
					LowerRight.Dock = DockStyle.None;
					LowerRight.ShowThumbs();
				}
			}
		}

		#endregion //END PROPERTY SplitView

		#region PUBLIC PROPERTY CHILDBODERSTYLE

		/// <summary>
		/// Gets or Sets the border styles of the split views.
		/// </summary>
		[Category("Appearance - Borders")]
		[Description("Gets or Sets the border styles of the split views.")]
		[DefaultValue(BorderStyle.FixedSingle)]
		public BorderStyle ChildBorderStyle
		{
			get { return ((SplitViewChildControl) Views[0]).BorderStyle; }
			set
			{
				foreach (SplitViewChildControl ev in this.Views)
				{
					ev.BorderStyle = value;
				}
			}
		}

		#endregion

		#region PUBLIC PROPERTY CHILDBORDERCOLOR

		/// <summary>
		/// Gets or Sets the border color of the split views.
		/// </summary>
		[Category("Appearance - Borders")]
		[Description("Gets or Sets the border color of the split views.")]
		[DefaultValue(typeof (Color), "ControlDark")]
		public Color ChildBorderColor
		{
			get { return ((SplitViewChildControl) Views[0]).BorderColor; }
			set
			{
				foreach (SplitViewChildControl ev in this.Views)
				{
					if (ev != null)
					{
						ev.BorderColor = value;
					}
				}
			}
		}

		#endregion

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == (int) WindowMessage.WM_SETFOCUS)
			{
				if (_ActiveView != null)
					_ActiveView.Focus();
			}
		}
	}
}

namespace Puzzle.Windows.Forms
{
	/// <summary>
	/// Represents which split view is currently active in the syntaxbox
	/// </summary>
	public enum ActiveView
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight,
	}
}