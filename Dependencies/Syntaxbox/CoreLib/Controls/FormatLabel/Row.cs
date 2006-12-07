// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System.Collections;

namespace Puzzle.Windows.Forms.FormatLabel
{
	public class Row
	{
		public int Width = 0;
		public int Height = 0;
		public int BottomPadd = 0;
		public ArrayList Words = new ArrayList();
		public bool RenderSeparator = false;
		public bool Visible = false;
		public int Top = 0;

		public Row()
		{
		}
	}
}