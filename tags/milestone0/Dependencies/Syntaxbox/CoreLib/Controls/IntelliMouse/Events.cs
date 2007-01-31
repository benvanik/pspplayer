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

namespace Puzzle.Windows.Forms.IntelliMouse
{
	public class ScrollEventArgs : EventArgs
	{
		public int DeltaX = 0;
		public int DeltaY = 0;
	}

	public delegate void ScrollEventHandler(object sender, ScrollEventArgs e);
}