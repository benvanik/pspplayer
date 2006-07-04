using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Utilities.Controls
{
	public class DoubleBufferedTreeView : TreeView
	{
		protected override void WndProc( ref Message m )
		{
			// Stop erase background message
			if( m.Msg == ( int )0x0014 )
			{
				// Set to null (ignore)
				m.Msg = ( int )0x0000;
			}

			base.WndProc( ref m );
		}
	}
}
