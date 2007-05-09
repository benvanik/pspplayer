// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Noxa.Emulation.Psp.Player
{
	class FeatureInfo
	{
		public string Name;
		public bool Enabled;
		public Verbosity VerbosityLevel;
		public FeatureInfo( string name )
		{
			Name = name;
			Enabled = true;
			VerbosityLevel = Verbosity.Verbose;
		}
	}

	partial class LogViewer : Form, ILogger
	{
		private FeatureInfo[] _features;

		public LogViewer()
		{
			InitializeComponent();

			string[] names = Enum.GetNames( typeof( Feature ) );
			_features = new FeatureInfo[ names.Length ];
			foreach( string name in names )
			{
				FeatureInfo feature = new FeatureInfo( name );
				int value = ( int )Enum.Parse( typeof( Feature ), name );
				_features[ value ] = feature;
			}

			Log.Instance = this;
		}

		#region ILogger Members

		public void WriteLine( Verbosity verbosity, Feature feature, string value )
		{
			FeatureInfo f = _features[ ( int )feature ];
			if( f.Enabled == false )
				return;
			if( ( int )verbosity > ( int )f.VerbosityLevel )
				return;
			Debug.WriteLine( string.Format( "{0}: {1}", feature, value ) );
		}

		#endregion
	}
}