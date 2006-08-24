// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Noxa.Emulation.Psp.Games;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Noxa.Emulation.Psp.Player.CrossMediaBar
{
	class GameBarItem : BarItem
	{
		protected GameInformation _info;

		public GameBarItem( GameInformation info )
			: base( info.Parameters.Title )
		{
			Debug.Assert( info != null );

			_info = info;
		}

		public override string Name
		{
			get
			{
				return base.Name;
			}
		}

		public override string Description
		{
			get
			{
				return base.Description;
			}
		}

		public GameInformation Game
		{
			get
			{
				return _info;
			}
		}

		public override void Create( Manager manager )
		{
			_resources.Add( manager.Video.CreateTextResource(
				new System.Drawing.Font( "Tahoma", 10.0f, System.Drawing.FontStyle.Bold ),
				this.Name,
				Color.White ) );
			_resources.Add( manager.Video.CreateTextResource(
				new System.Drawing.Font( "Tahoma", 12.0f, System.Drawing.FontStyle.Bold ),
				this.Name,
				Color.White ) );

			_resources.Add( manager.IconBackground );
			if( _info.Icon != null )
			{
				VideoResource res = manager.Video.CreateImageResource( _info.Icon );
				_resources.Add( res );
				_resources.Add( res );
			}
			else
			{
				_resources.Add( manager.InvalidIcon );
				_resources.Add( manager.InvalidIcon );
			}
		}

		public override SizeF Draw( float x, float y, bool isSelected )
		{
			int textIndex = ( isSelected == true ) ? 1 : 0;
			int iconIndex = ( isSelected == true ) ? 4 : 3;

			// 144x80
			float iconWidth = 48 * 2;
			float iconHeight = 27 * 2;

			float textPad = ( isSelected == true ) ? 1 : 0;
			float iconPad = ( isSelected == true ) ? 3 : 0;
			float bgPad = 2;

			_resources[ textIndex ].Draw( x + iconWidth + 8, y + iconHeight / 2 - 6 - textPad );
			_resources[ 2 ].Draw( x - iconPad - bgPad, y - iconPad - bgPad, iconWidth + iconPad * 2 + bgPad * 2, iconHeight + iconPad * 2 + bgPad * 2 );
			_resources[ iconIndex ].Draw( x - iconPad, y - iconPad, iconWidth + iconPad * 2, iconHeight + iconPad * 2 );

			SizeF size = new SizeF( iconWidth + 8 + _resources[ textIndex ].Width, iconHeight + 2 );

			return size;
		}
	}
}
