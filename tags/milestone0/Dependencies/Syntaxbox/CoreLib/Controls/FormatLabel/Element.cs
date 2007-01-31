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
using System.Globalization;

namespace Puzzle.Windows.Forms.FormatLabel
{
	public enum TextEffect
	{
		None = 0,
		Outline,
		ShadowRB,
		ShadowLB,
		ShadowRT,
		ShadowLT,
	}

	public class Element
	{
		protected string _Tag = "";
		protected string _TagName = "";
		public Word[] words = null;
		public string Text = "";
		public Font Font = null;
		public Color BackColor = Color.Black;
		public Color ForeColor = Color.Black;
		public Color EffectColor = Color.Black;
		public TextEffect Effect = 0;

		public bool NewLine = false;
		public Element Link = null;

		public Element()
		{
		}

		public string TagName
		{
			get { return _TagName; }
		}


		public string Tag
		{
			get { return _Tag; }
			set
			{
				_Tag = value.ToLower(CultureInfo.InvariantCulture);
				_Tag = _Tag.Replace("\t", " ");
				if (_Tag.IndexOf(" ") >= 0)
				{
					_TagName = _Tag.Substring(0, _Tag.IndexOf(" "));
				}
				else
				{
					_TagName = _Tag;
				}
			}

		}
	}
}