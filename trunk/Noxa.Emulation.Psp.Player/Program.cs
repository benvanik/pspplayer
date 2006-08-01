using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Noxa.Emulation.Psp.Player
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );

			Host host = new Host();
			Application.Run( host.Player );
		}
	}
}