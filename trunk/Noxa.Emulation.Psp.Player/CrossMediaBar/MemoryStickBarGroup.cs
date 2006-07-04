using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Games;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Noxa.Emulation.Psp.Player.CrossMediaBar
{
	class MemoryStickBarGroup : BarGroup
	{
		public MemoryStickBarGroup( List<GameInformation> games )
			: base( "Memory Stick" )
		{
			Debug.Assert( games != null );

			foreach( GameInformation game in games )
				_items.Add( new GameBarItem( game ) );
		}

		public override void Create( Manager manager )
		{
			_resources.Add( manager.Video.CreateTextResource(
				new System.Drawing.Font( "Tahoma", 10.0f, System.Drawing.FontStyle.Bold ),
				this.Name,
				System.Drawing.Color.White ) );
			_resources.Add( manager.Video.CreateTextResource(
				new System.Drawing.Font( "Tahoma", 10.0f, System.Drawing.FontStyle.Bold ),
				this.Name,
				System.Drawing.Color.White ) );

			byte[] bytes = Properties.Resources.MemoryStickIcon;
			_resources.Add( manager.Video.CreateImageResource( new MemoryStream( bytes, 0, bytes.Length, false, false ) ) );
			bytes = Properties.Resources.SelectedMemoryStickIcon;
			_resources.Add( manager.Video.CreateImageResource( new MemoryStream( bytes, 0, bytes.Length, false, false ) ) );

			base.Create( manager );
		}

		protected float _itemHeight = 0;

		public override SizeF Draw( float x, float y, BarGroup selectedGroup, BarItem selectedItem )
		{
			float iconSize = 48;

			int textIndex = ( selectedGroup == this ) ? 1 : 0;
			int iconIndex = ( selectedGroup == this ) ? 3 : 2;

			SizeF size = new SizeF( Math.Max( iconSize + 4, _resources[ textIndex ].Width + 4 ), iconSize + 20 + 5 );

			float iconX;
			float textX;
			if( _resources[ textIndex ].Width < iconSize )
			{
				iconX = 0;
				textX = ( iconSize / 2.0f ) - _resources[ textIndex ].Width / 2.0f;
			}
			else
			{
				iconX = ( _resources[ textIndex ].Width / 2.0f ) - ( iconSize / 2.0f );
				textX = 0;
			}

			_resources[ textIndex ].Draw( x + textX, y + iconSize );
			_resources[ iconIndex ].Draw( x + iconX, y, iconSize, iconSize );

			if( selectedGroup == this )
			{
				int selectedIndex = _items.IndexOf( selectedItem );
				float selectedY = ( _itemHeight + 4 ) * selectedIndex - ( _itemHeight + 4 );

				float itemX = x + 10;
				float itemY = y + size.Height + 2 - selectedY;

				int previousIndex = Math.Max( 0, ( selectedIndex - 1 ) );
				itemY += ( _itemHeight + 4 ) * previousIndex;

				for( int m = previousIndex; m < _items.Count; m++ )
				{
					BarItem item = _items[ m ];

					SizeF itemSize = item.Draw( itemX, itemY, ( item == selectedItem ) );
					if( _itemHeight <= 0 )
						_itemHeight = itemSize.Height;

					itemY += itemSize.Height + 4;
				}
			}

			return size;
		}
	}
}
