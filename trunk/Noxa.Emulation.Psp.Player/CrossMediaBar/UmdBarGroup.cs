// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Games;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace Noxa.Emulation.Psp.Player.CrossMediaBar
{
	class UmdBarGroup : BarGroup
	{
		protected GameInformation _game;

		public UmdBarGroup( GameInformation game )
			: base( "UMD" )
		{
			Debug.Assert( game != null );

			_game = game;

			_items.Add( new GameBarItem( game ) );
		}

		public override string Name
		{
			get
			{
				if( ( _game.Parameters.DiscID != null ) &&
					( _game.Parameters.DiscID.Length > 0 ) )
					return _game.Parameters.DiscID;
				else
					return base.Name;
			}
		}

		public override Stream Background
		{
			get
			{
				return _game.Background;
			}
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

			byte[] bytes = Properties.Resources.UmdIcon;
			_resources.Add( manager.Video.CreateImageResource( new MemoryStream( bytes, 0, bytes.Length, false, false ) ) );
			bytes = Properties.Resources.SelectedUmdIcon;
			_resources.Add( manager.Video.CreateImageResource( new MemoryStream( bytes, 0, bytes.Length, false, false ) ) );

			base.Create( manager );
		}

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
				//float itemX = x + 10;
				//float itemY = y + size.Height + 2;
				float itemX = 15;
				float itemY = y + size.Height + ( ( 272 - ( y + size.Height ) ) / 2 - size.Height / 2 );

				// Should only have one item
				for( int m = 0; m < _items.Count; m++ )
				{
					BarItem item = _items[ m ];

					SizeF itemSize = item.Draw( itemX, itemY, ( item == selectedItem ) );

					itemY += itemSize.Height + 4;
				}
			}

			return size;
		}
	}
}
