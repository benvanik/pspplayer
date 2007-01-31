// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Puzzle.Windows.Forms.CoreLib
{
	[ToolboxItem(true)]
	public class RegionHandler : Component
	{
		private Container components = null;

		#region PUBLIC PROPERTY TRANSPARENCYKEY

		private Color _TransparencyKey = Color.FromArgb(255, 0, 255);

		public Color TransparencyKey
		{
			get { return _TransparencyKey; }
			set { _TransparencyKey = value; }
		}

		#endregion

		#region PUBLIC PROPERTY CONTROL

		private Control _Control;

		public Control Control
		{
			get { return _Control; }
			set { _Control = value; }
		}

		#endregion

		#region PUBLIC PROPERTY MASKIMAGE

		private Bitmap _MaskImage;

		public Bitmap MaskImage
		{
			get { return _MaskImage; }
			set { _MaskImage = value; }
		}

		#endregion

		public void ApplyRegion(Control Target, Bitmap MaskImage, Color TransparencyKey)
		{
			this.Control = Target;
			this.MaskImage = MaskImage;
			this.TransparencyKey = TransparencyKey;
			ApplyRegion();
		}


		public void ApplyRegion()
		{
			Region r = new Region(new Rectangle(0, 0, MaskImage.Width, MaskImage.Height));

			for (int y = 0; y < this.MaskImage.Height; y++)
				for (int x = 0; x < this.MaskImage.Width; x++)
				{
					if (this.MaskImage.GetPixel(x, y) == this.TransparencyKey)
					{
						r.Exclude(new Rectangle(x, y, 1, 1));
					}
				}

			Control.Region = r;
			Control.BackgroundImage = this.MaskImage;
		}

		public RegionHandler(IContainer container)
		{
			container.Add(this);
			InitializeComponent();

		}

		public RegionHandler()
		{
			InitializeComponent();
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}

		#endregion
	}
}