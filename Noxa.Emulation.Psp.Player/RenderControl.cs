// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Emulation.Psp.Player
{
	public partial class RenderControl : Control
	{
		public const int CS_VREDRAW = 0x0001;
		public const int CS_HREDRAW = 0x0002;
		public const int CS_OWNDC = 32;

		public RenderControl()
		{
			InitializeComponent();

			this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			this.SetStyle( ControlStyles.Opaque, true );
			this.SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
			this.SetStyle( ControlStyles.ResizeRedraw, true );
			this.SetStyle( ControlStyles.UserPaint, true );

			this.CreateParams.ClassStyle |= CS_OWNDC | CS_VREDRAW | CS_HREDRAW;
		}
	}
}
