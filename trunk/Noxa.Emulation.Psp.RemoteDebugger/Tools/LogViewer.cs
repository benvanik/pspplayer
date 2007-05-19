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
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp.RemoteDebugger.Tools
{
	partial class LogViewer : ToolPane, ILogger
	{
		public EmuDebugger Debugger;

		private FeatureInfo[] _features;

		private Verbosity _globalVerbosity;
		private bool _debugWrite;
		private bool _debugEnabled;

		private List<ListViewItem> _lines;

		public LogViewer()
		{
			InitializeComponent();
		}

		public LogViewer( EmuDebugger debugger )
			: this()
		{
			this.Debugger = debugger;

			string[] names = Enum.GetNames( typeof( Feature ) );
			_features = new FeatureInfo[ names.Length ];
			foreach( string name in names )
			{
				FeatureInfo feature = new FeatureInfo( name );
				int value = ( int )Enum.Parse( typeof( Feature ), name );
				_features[ value ] = feature;
			}

			Log.Instance = this;

			_addLine = new AddLineDelegate( this.AddLine );

			listView_Resize( this, EventArgs.Empty );
			criticalToolStripMenuItem.ForeColor = Color.DarkRed;
			verboseToolStripMenuItem.ForeColor = Color.DarkGreen;
			everythingToolStripMenuItem.ForeColor = Color.Blue;

			_lines = new List<ListViewItem>( 10000 );

			if( ( System.Diagnostics.Debugger.IsAttached == true ) &&
				( System.Diagnostics.Debugger.IsLogging() == true ) )
			{
				_debugEnabled = true;
			}
			else
			{
				_debugEnabled = false;
			}
			debugWriteToolStripButton.Visible = _debugEnabled;
			toolStripSeparator3.Visible = _debugEnabled;

			this.LoadSettings();

			this.Refilter();
		}

		private void LoadSettings()
		{
			_globalVerbosity = Verbosity.Normal;
			_debugWrite = false;

			this.UpdateGlobalVerbosity();
			debugWriteToolStripButton.Checked = _debugWrite;
			debugWriteToolStripButton.Invalidate();
		}

		private void SaveSettings()
		{
		}

		#region ILogger Members

		private delegate void AddLineDelegate( Verbosity verbosity, FeatureInfo feature, ListViewItem item );
		private AddLineDelegate _addLine;

		private void AddLine( Verbosity verbosity, FeatureInfo feature, ListViewItem item )
		{
			try
			{

				if( ( int )verbosity > ( int )_globalVerbosity )
					return;

				bool autoScroll = true;
				if( listView.Items.Count > 0 )
					autoScroll = listView.Items[ listView.Items.Count - 1 ].Focused;

				listView.BeginUpdate();
				listView.Items.Add( item );
				if( autoScroll == true )
				{
					item.EnsureVisible();
					item.Focused = true;
				}
				listView.EndUpdate();

			}
			catch
			{
				//Debugger.Break();
			}
		}

		public void WriteLine( Verbosity verbosity, Feature feature, string value )
		{
			if( ( _debugEnabled == true ) &&
				( _debugWrite == true ) )
				Debug.WriteLine( string.Format( "{0}: {1}", feature, value ) );

			FeatureInfo f = _features[ ( int )feature ];
			if( f.Enabled == false )
				return;

			ListViewItem item = new ListViewItem( new string[] { f.Name, value } );
			switch( verbosity )
			{
				case Verbosity.Critical:
					item.SubItems[ 0 ].ForeColor = Color.DarkRed;
					break;
				case Verbosity.Normal:
					break;
				case Verbosity.Verbose:
					item.SubItems[ 0 ].ForeColor = Color.DarkGreen;
					break;
			}
			item.Tag = new object[]{ verbosity, f };

			lock( _lines )
				_lines.Add( item );

			if( this.InvokeRequired == true )
				this.BeginInvoke( _addLine, verbosity, f, item );
			else
				AddLine( verbosity, f, item );
		}

		#endregion

		private void listView_Resize( object sender, EventArgs e )
		{
			int clientWidth = listView.ClientSize.Width;
			clientWidth -= columnFeature.Width + 4;
			columnValue.Width = clientWidth;
		}

		private void Refilter()
		{
			listView.BeginUpdate();
			listView.Items.Clear();
			lock( _lines )
			{
				foreach( ListViewItem item in _lines )
				{
					Verbosity verbosity = ( Verbosity )( ( object[] )item.Tag )[ 0 ];
					if( ( int )verbosity > ( int )_globalVerbosity )
						continue;

					listView.Items.Add( item );
				}
			}
			if( listView.Items.Count > 0 )
			{
				ListViewItem lastItem = listView.Items[ listView.Items.Count - 1 ];
				lastItem.Focused = true;
				lastItem.EnsureVisible();
			}
			listView.EndUpdate();
		}

		private void UpdateGlobalVerbosity()
		{
			verbosityToolStripSplitButton.Text = _globalVerbosity.ToString();
			switch( _globalVerbosity )
			{
				case Verbosity.Critical:
					verbosityToolStripSplitButton.ForeColor = Color.DarkRed;
					break;
				case Verbosity.Normal:
					verbosityToolStripSplitButton.ForeColor = SystemColors.ControlText;
					break;
				case Verbosity.Verbose:
					verbosityToolStripSplitButton.ForeColor = Color.DarkGreen;
					break;
				case Verbosity.Everything:
					verbosityToolStripSplitButton.ForeColor = Color.Blue;
					break;
			}
			this.Refilter();
		}

		private void criticalToolStripMenuItem_Click( object sender, EventArgs e )
		{
			_globalVerbosity = Verbosity.Critical;
			this.UpdateGlobalVerbosity();
		}

		private void normalToolStripMenuItem_Click( object sender, EventArgs e )
		{
			_globalVerbosity = Verbosity.Normal;
			this.UpdateGlobalVerbosity();
		}

		private void verboseToolStripMenuItem_Click( object sender, EventArgs e )
		{
			_globalVerbosity = Verbosity.Verbose;
			this.UpdateGlobalVerbosity();
		}

		private void everythingToolStripMenuItem_Click( object sender, EventArgs e )
		{
			_globalVerbosity = Verbosity.Everything;
			this.UpdateGlobalVerbosity();
		}

		private void copyToolStripButton_Click( object sender, EventArgs e )
		{
			StringBuilder sb = new StringBuilder( 100000 );
			lock( _lines )
			{
				foreach( ListViewItem item in _lines )
				{
					Verbosity verbosity = ( Verbosity )( ( object[] )item.Tag )[ 0 ];
					FeatureInfo feature = ( FeatureInfo )( ( object[] )item.Tag )[ 1 ];
					string text = item.SubItems[ 1 ].Text;
					sb.AppendFormat( "[{0}] {1}: {2}{3}", verbosity, feature.Name, text, Environment.NewLine );
				}
			}
			Clipboard.SetText( sb.ToString(), TextDataFormat.Text );
		}

		private void clearToolStripButton_Click( object sender, EventArgs e )
		{
			lock( _lines )
				_lines.Clear();
			this.Refilter();
		}

		private void saveAsTextToolStripMenuItem_Click( object sender, EventArgs e )
		{
			saveFileDialog.DefaultExt = ".txt";
			saveFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
			if( saveFileDialog.ShowDialog() != DialogResult.OK )
				return;

			using( StreamWriter writer = new StreamWriter( saveFileDialog.FileName ) )
			{
				lock( _lines )
				{
					foreach( ListViewItem item in _lines )
					{
						Verbosity verbosity = ( Verbosity )( ( object[] )item.Tag )[ 0 ];
						FeatureInfo feature = ( FeatureInfo )( ( object[] )item.Tag )[ 1 ];
						string text = item.SubItems[ 1 ].Text;
						writer.WriteLine( "[{0}] {1}: {2}", verbosity, feature.Name, text );
					}
				}
			}
		}

		private void saveAsHtmlToolStripMenuItem_Click( object sender, EventArgs e )
		{
			saveFileDialog.DefaultExt = ".html";
			saveFileDialog.Filter = "HTML Files|*.html|All Files|*.*";
			if( saveFileDialog.ShowDialog() != DialogResult.OK )
				return;
		}

		private void saveAsXmlToolStripMenuItem_Click( object sender, EventArgs e )
		{
			saveFileDialog.DefaultExt = ".xml";
			saveFileDialog.Filter = "XML Files|*.xml|All Files|*.*";
			if( saveFileDialog.ShowDialog() != DialogResult.OK )
				return;
		}

		private void debugWriteToolStripButton_Click( object sender, EventArgs e )
		{
			debugWriteToolStripButton.Checked = !debugWriteToolStripButton.Checked;
			_debugWrite = debugWriteToolStripButton.Checked;
			debugWriteToolStripButton.Invalidate();
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}
	}

	class FeatureInfo
	{
		public string Name;
		public bool Enabled;
		public FeatureInfo( string name )
		{
			Name = name;
			Enabled = true;
		}
	}
}