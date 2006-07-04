using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Noxa.Emulation.Psp.Player.CrossMediaBar
{
	class BarGroup
	{
		protected string _name;
		protected Image _icon;
		protected List<BarItem> _items = new List<BarItem>();
		protected List<VideoResource> _resources = new List<VideoResource>();
		protected VideoResource _backgroundResource;

		public BarGroup( string name )
		{
			Debug.Assert( name != null );
			_name = name;
		}

		public virtual string Name
		{
			get
			{
				return _name;
			}
		}

		public virtual Image Icon
		{
			get
			{
				return _icon;
			}
		}

		public virtual Image SelectedIcon
		{
			get
			{
				return _icon;
			}
		}

		public virtual Stream Background
		{
			get
			{
				return null;
			}
		}

		public virtual VideoResource BackgroundResource
		{
			get
			{
				if( this.Background == null )
					return null;
				return _backgroundResource;
			}
		}

		public IList<BarItem> Items
		{
			get
			{
				return _items;
			}
		}

		private static int BarItemComparison( BarItem a, BarItem b )
		{
			return string.Compare( a.Name, b.Name );
		}

		public void Sort()
		{
			_items.Sort( new Comparison<BarItem>( BarItemComparison ) );
		}

		public IList<VideoResource> Resources
		{
			get
			{
				return _resources;
			}
		}

		public virtual void Create( Manager manager )
		{
			foreach( BarItem item in _items )
				item.Create( manager );

			if( this.Background != null )
				_backgroundResource = manager.Video.CreateImageResource( this.Background );
		}

		public virtual SizeF Draw( float x, float y, BarGroup selectedGroup, BarItem selectedItem )
		{
			return SizeF.Empty;
		}
	}
}
