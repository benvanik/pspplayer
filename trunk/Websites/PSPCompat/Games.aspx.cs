using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

public partial class Games : System.Web.UI.Page
{
	protected void Page_Load( object sender, EventArgs e )
	{
		this.EnableViewState = false;

		char letterQuery = ( char )0;
		{
			string letter = this.Request.QueryString[ "letter" ];
			if( ( letter != null ) &&
				( letter.Length == 1 ) )
				letterQuery = letter[ 0 ];
		}

		{
			TableRow row = new TableRow();
			TableCell cell;

			cell = new TableCell();
			cell.Text = "Games";
			row.Cells.Add( cell );

			row.TableSection = TableRowSection.TableHeader;
		}

		using( Database db = new Database() )
		{
			foreach( Game game in db.ListGames( letterQuery ) )
			{
				TableRow row;
				TableCell cell;

				{
					row = new TableRow();

					cell = new TableCell();
					cell.ColumnSpan = 5;
					cell.Text = game.Title;
					row.Cells.Add( cell );

					gamesTable.Rows.Add( row );
				}

				foreach( GameRelease release in game.Releases )
				{
					row = new TableRow();

					cell = new TableCell();
					cell.Text = "RGN";
					row.Cells.Add( cell );

					cell = new TableCell();
					cell.Text = release.DiscID;
					row.Cells.Add( cell );

					cell = new TableCell();
					cell.Text = ( release.Title != null ) ? release.Title : "";
					row.Cells.Add( cell );

					cell = new TableCell();
					if( release.Website == null )
						cell.Text = "google";
					else
						cell.Text = "site";
					row.Cells.Add( cell );

					cell = new TableCell();
					cell.Text = ( release.ReleaseDate != null ) ? release.ReleaseDate.Value.ToShortDateString() : "";
					row.Cells.Add( cell );

					cell = new TableCell();
					cell.Text = ( release.SystemVersion != null ) ? string.Format( "FW{0}", release.SystemVersion.Value ) : "";
					row.Cells.Add( cell );

					gamesTable.Rows.Add( row );
				}
			}
		}
	}
}
