// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System.Drawing;

namespace Puzzle.Windows.Forms.FormatLabel
{
	public class Word
	{
		public Image Image = null;
		public string Text = "";
		public int Width = 0;
		public int Height = 0;
		public Element Element = null;
		public Rectangle ScreenArea = new Rectangle(0, 0, 0, 0);
		//	public bool Link=false;

		public Word()
		{
		}
	}
}