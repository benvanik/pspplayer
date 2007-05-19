// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Noxa.Emulation.Psp.Debugging.Statistics;

namespace Noxa.Emulation.Psp.RemoteDebugger.Tools
{
	partial class StatisticsViewer : Noxa.Emulation.Psp.RemoteDebugger.Tools.ToolPane
	{
		class CounterGroup
		{
			public readonly CounterSource Source;
			public readonly string Name;
			public readonly Counter[] Counters;
			public ListViewGroup ListViewGroup;
			public CounterItem[] Items;

			public CounterGroup( CounterSource source )
			{
				this.Source = source;
				this.Name = source.Name;
				this.Counters = source.GetCounters();
			}
		}

		class CounterItem : ListViewItem
		{
			public Counter Counter;
		}

		public EmuDebugger Debugger;
		public CounterSink Sink;
		private CounterGroup[] Groups;

		private Font _nameFont;
		private Font _valueFont;

		public StatisticsViewer()
		{
			InitializeComponent();
		}

		public StatisticsViewer( EmuDebugger debugger )
			: this()
		{
			this.Debugger = debugger;
			this.Sink = debugger.Host.Counters;

			this.toolStrip1.Enabled = false;

			this._nameFont = new Font( "Tahoma", 8.0f, FontStyle.Regular );
			this._valueFont = new Font( "Courier New", 8.0f, FontStyle.Regular );

			Bitmap image = Properties.Resources.StatisticsIcon as Bitmap;
			this.Icon = Icon.FromHandle( image.GetHicon() );
		}

		public override void OnStarted()
		{
			this.toolStrip1.Enabled = true;

			List<CounterGroup> groups = new List<CounterGroup>();
			CounterSource[] sources = this.Sink.GetSources();
			foreach( CounterSource source in sources )
				groups.Add( new CounterGroup( source ) );
			this.Groups = groups.ToArray();

			this.listView1.BeginUpdate();
			foreach( CounterGroup group in this.Groups )
				this.AddCounters( group );
			this.listView1.EndUpdate();
		}

		public override void OnStopped()
		{
			this.ClearCounters();

			this.toolStrip1.Enabled = false;
		}

		private void AddCounters( CounterGroup group )
		{
			group.ListViewGroup = this.listView1.Groups.Add( group.Name, group.Name );

			List<CounterItem> items = new List<CounterItem>( group.Counters.Length );
			foreach( Counter counter in group.Counters )
			{
				CounterItem item = new CounterItem();
				item.Text = counter.Name;
				item.ToolTipText = counter.Description;
				item.Group = group.ListViewGroup;
				item.Font = _nameFont;
				item.Counter = counter;
				item.UseItemStyleForSubItems = false;

				ListViewItem.ListViewSubItem valueItem = new ListViewItem.ListViewSubItem( item, "-" );
				valueItem.Font = _valueFont;
				item.SubItems.Add( valueItem );
				ListViewItem.ListViewSubItem deltaItem = new ListViewItem.ListViewSubItem( item, "-" );
				deltaItem.Font = _valueFont;
				item.SubItems.Add( deltaItem );
				
				this.listView1.Items.Add( item );
				items.Add( item );
			}
			group.Items = items.ToArray();

			this.timer1.Enabled = true;
		}

		private void ClearCounters()
		{
			this.timer1.Enabled = false;

			this.listView1.Items.Clear();
		}

		private void timer1_Tick( object sender, EventArgs e )
		{
			try
			{
				foreach( CounterGroup group in this.Groups )
					group.Source.Sample();

				this.listView1.BeginUpdate();
				foreach( CounterGroup group in this.Groups )
				{
					foreach( CounterItem item in group.Items )
					{
						item.SubItems[ 1 ].Text = string.Format( "{0}", item.Counter.LastValue );
						double delta = item.Counter.Delta;
						item.SubItems[ 2 ].Text = string.Format( "{0}/s", delta );
						if( delta < 0.0 )
							item.SubItems[ 2 ].ForeColor = Color.Red;
						else if( delta == 0.0 )
							item.SubItems[ 2 ].ForeColor = SystemColors.ControlText;
						else
							item.SubItems[ 2 ].ForeColor = Color.Green;
					}
				}
				this.listView1.EndUpdate();
			}
			catch( System.Net.Sockets.SocketException ex )
			{
				// Assume we got dc/ed
				this.Debugger.OnConnectionLost();

				this.timer1.Enabled = false;
			}
		}
	}
}

