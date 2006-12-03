using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Noxa.Utilities.Controls
{
	public static class IconUtilities
	{
		public static Icon ConvertToIcon( Image image )
		{
			Bitmap bitmap = image as Bitmap;
			if( bitmap == null )
				bitmap = new Bitmap( image );
			IntPtr iconHandle = bitmap.GetHicon();
			return Icon.FromHandle( iconHandle );
		}
	}
}
