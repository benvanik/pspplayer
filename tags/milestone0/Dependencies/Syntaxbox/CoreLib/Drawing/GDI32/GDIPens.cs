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
using System.Drawing;
using Puzzle.Windows;

namespace Puzzle.Drawing.GDI
{
	public class GDIPen : GDIObject
	{
		public IntPtr hPen;

		public GDIPen(Color color, int width)
		{
			hPen = NativeMethods.CreatePen(0, width, NativeMethods.ColorToInt(color));
			Create();
		}

		protected override void Destroy()
		{
			if (hPen != (IntPtr) 0)
				NativeMethods.DeleteObject(hPen);
			base.Destroy();
			hPen = (IntPtr) 0;
		}

		protected override void Create()
		{
			base.Create();
		}
	}
}