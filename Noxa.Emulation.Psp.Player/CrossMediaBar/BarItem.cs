using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace Noxa.Emulation.Psp.Player.CrossMediaBar
{
	class BarItem
	{
		protected string _name;
		protected Image _icon;
		protected List<VideoResource> _resources = new List<VideoResource>();

		public BarItem( string name )
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

		public virtual string Description
		{
			get
			{
				return null;
			}
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
		}

		public virtual SizeF Draw( float x, float y, bool isSelected )
		{
			return SizeF.Empty;
		}
	}
}
