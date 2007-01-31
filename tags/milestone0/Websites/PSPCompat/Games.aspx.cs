// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

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
using System.Diagnostics;

public partial class Games : System.Web.UI.Page
{
	#region Icons
	
	private static List<Region> _regionIconRanks = new List<Region>( new Region[]{
		Region.America, Region.Japan, Region.Unknown
	} );
	private static long? FindReleaseIcon( Game game )
	{
		long? ret = null;
		Region retRegion = Region.Unknown;

		foreach( GameRelease release in game.Releases )
		{
			if( release.HasIcon == true )
			{
				if( ret == null )
				{
					ret = release.ReleaseID;
					if( release.Region.HasValue == true )
						retRegion = ( Region )release.Region;
					else
						retRegion = Region.Unknown;
				}
				else
				{
					if( release.Region.HasValue == false )
						continue;
					Region region = ( Region )release.Region.Value;
					int oldIndex = ( _regionIconRanks.Contains( retRegion ) ? _regionIconRanks.IndexOf( retRegion ) : -1 );
					int newIndex = ( _regionIconRanks.Contains( region ) ? _regionIconRanks.IndexOf( region ) : -1 );
					if( newIndex > oldIndex )
					{
						ret = release.ReleaseID;
						retRegion = region;
					}
				}
			}
		}

		return ret;
	}
	
	#endregion

	public const string WikiRoot = "http://dev.ixtli.com/pspdev/wiki/";

	private List<CheckBox> _checkboxes = new List<CheckBox>();

	protected void Page_Load( object sender, EventArgs e )
	{
		UserSession session = UserSession.FromContext( this );
		if( session == null )
			throw new ApplicationException( "Unable to create session for user." );

		char letterQuery = ( char )0;
		{
			string letter = this.Request.QueryString[ "letter" ];
			if( ( letter != null ) &&
				( letter.Length == 1 ) )
				letterQuery = letter[ 0 ];
		}

		bool showRecent = false;
		{
			if( this.Request.QueryString[ "recent" ] != null )
				showRecent = true;
		}

		// TODO: support show recent list
		if( showRecent == true )
		{
			this.InfoLabel.Text = "Not yet supported.";
			this.InfoLabel.Visible = true;
			return;
		}

		bool showPendingApproval = false;
		{
			string action = this.Request.QueryString[ "action" ];
			if( action != null )
				action = action.ToLowerInvariant();
			if( action == "showpendingapproval" )
				showPendingApproval = true;
		}

		this.AdminToolsLiteral.Visible = session.Database.CanEdit;
		if( showPendingApproval == true )
		{
			this.ApproveAllButton.Visible = session.Database.CanEdit;
			this.ApproveSelectedButton.Visible = session.Database.CanEdit;
		}
		else
		{
			this.ApproveAllButton.Visible = false;
			this.ApproveSelectedButton.Visible = false;
		}

		// Cells:
		// Icon    | Title               | N releases
		//			 RGN | [Title] | [2006/05/05] | Links | FW version | Supported

		gamesTable.Width = new Unit( 80.0, UnitType.Percentage );
		{
			TableRow row = new TableRow();
			TableCell cell;

			cell = new TableCell();
			//cell.Text = "Games";
			row.Cells.Add( cell );

			row.TableSection = TableRowSection.TableHeader;
			gamesTable.Rows.Add( row );
		}

		List<Game> games = session.Database.ListGames( letterQuery, showPendingApproval );
		if( games.Count == 0 )
		{
			this.InfoLabel.Text = "No results found.";
			this.InfoLabel.Visible = true;
			return;
		}

		int columnCount = 7;

		foreach( Game game in games )
		{
			TableRow row;
			TableCell cell;

			{
				row = new TableRow();

				cell = new TableCell();
				long? iconReleaseId = FindReleaseIcon( game );
				if( iconReleaseId.HasValue == true )
					cell.Text = string.Format( "<img src=\"{0}.{1}.gameicon?size=small\" style=\"margin: 0\" />", game.GameID, iconReleaseId.Value );
				row.Cells.Add( cell );
				cell.Width = new Unit( 98, UnitType.Pixel );
				cell.VerticalAlign = VerticalAlign.Top;
				cell.HorizontalAlign = HorizontalAlign.Left;
				cell.RowSpan = 1 + game.Releases.Count;
				row.Cells.Add( cell );

				cell = new TableCell();
				cell.ColumnSpan = columnCount;
				string mobyLink = string.Empty;
				if( game.Website != null )
					mobyLink = string.Format( "<a href=\"{0}\" alt=\"Lookup on MobyGames\" style=\"border: 0; \"><img src=\"Resources/MobyGames.png\" style=\"vertical-align: bottom\" /></a>&nbsp;", game.Website );
				else
					mobyLink = "<img src=\"Resources/Blank.png\" />&nbsp;";
				string googleQuery = string.Format( "http://www.google.com/search?q={0}+PSP", Server.UrlPathEncode( game.Title ) );
				string googleLink = string.Format( "<a href=\"{0}\" alt=\"Search on Google\" style=\"border: 0; \"><img src=\"Resources/Google.png\" style=\"vertical-align: bottom\" /></a>&nbsp;", googleQuery );
				cell.Text = string.Format( "{0}{1}<a href=\"GameView.aspx?gameId={2}\">{3}</a>", mobyLink, googleLink, game.GameID, game.Title );
				cell.Style.Add( HtmlTextWriterStyle.FontWeight, "bold" );
				row.Cells.Add( cell );

				gamesTable.Rows.Add( row );
			}

			foreach( GameRelease release in game.Releases )
			{
				row = new TableRow();

				cell = new TableCell();
				cell.Text = "&nbsp;";
				row.Cells.Add( cell );

				cell = new TableCell();
				int regionId = 0;
				string regionName = "Unknown";
				string regionUrl = "";
				if( release.Region.HasValue == true )
				{
					Region region = ( Region )release.Region.Value;
					regionId = release.Region.Value;
					regionName = region.ToString();
					switch( region )
					{
						case Region.America:
							regionUrl = "http://en.wikipedia.org/wiki/United_States";
							break;
						case Region.Australia:
							regionUrl = "http://en.wikipedia.org/wiki/Australia";
							break;
						case Region.Europe:
							regionUrl = "http://en.wikipedia.org/wiki/Europe";
							break;
						case Region.Japan:
							regionUrl = "http://en.wikipedia.org/wiki/Japan";
							break;
						case Region.SouthKorea:
							regionUrl = "http://en.wikipedia.org/wiki/South_korea";
							break;
						default:
							regionId = 0;
							break;
					}
				}
				cell.Text = string.Format( "<a href=\"{0}\" style=\"text-decoration: none; border: 0;\"><img src=\"Resources/Flags/Flag{1}.png\" alt=\"{2}\" border=\"0\"/></a>",
					regionUrl, regionId, regionName );
				cell.Width = new Unit( 1, UnitType.Percentage );
				row.Cells.Add( cell );

				cell = new TableCell();
				string googleServer = "http://www.google.com/search?";
				if( release.Region.HasValue == true )
				{
					switch( ( Region )release.Region )
					{
						case Region.Japan:
							googleServer = "http://www.google.com/search?hl=ja&";
							break;
					}
				}
				string googleQuery = string.Format( "{0}q={1}+OR+{2}+PSP",
					googleServer, Server.UrlPathEncode( release.Title ), Server.UrlPathEncode( game.Title ) );
				string links = string.Format( "<a href=\"{0}\" style=\"border: 0\"><img src=\"Resources/Google.png\" alt=\"Search on Google\" /></a>", googleQuery );
				if( release.Website != null )
				{
					string linkIcon;
					linkIcon = "Resources/Link.png";
					links += string.Format( "<a href=\"{0}\" style=\"border: 0\"><img src=\"{1}\" alt=\"Listed Website\" /></a>", release.Website, linkIcon );
				}
				cell.Text = links;
				cell.Width = new Unit( 1, UnitType.Percentage );
				row.Cells.Add( cell );

				cell = new TableCell();
				cell.Text = ( release.Title != null ) ? release.Title : "";
				row.Cells.Add( cell );

				cell = new TableCell();
				cell.Text = ( release.ReleaseDate != null ) ? release.ReleaseDate.Value.ToShortDateString() : "";
				cell.Width = new Unit( 10.0, UnitType.Percentage );
				row.Cells.Add( cell );

				cell = new TableCell();
				if( release.SystemVersion.HasValue == true )
				{
					string fw = string.Format( "FW{0:0.0}", release.SystemVersion.Value );
					cell.Text = string.Format( "<a href=\"{0}{1}\">{1}</a>", WikiRoot, fw );
				}
				cell.Width = new Unit( 10.0, UnitType.Percentage );
				row.Cells.Add( cell );

				cell = new TableCell();
				cell.Text = "Sup";
				cell.Width = new Unit( 10.0, UnitType.Percentage );
				row.Cells.Add( cell );

				if( showPendingApproval == true )
				{
					cell = new TableCell();
					CheckBox approveCheckbox = new CheckBox();
					approveCheckbox.Text = "Approve";
					approveCheckbox.ToolTip = "Approve the selected release.";
					approveCheckbox.ID = string.Format( "ReleaseApprove.{0}.{1}", game.GameID, release.ReleaseID );
					approveCheckbox.InputAttributes.Add( "group", "1" );
					_checkboxes.Add( approveCheckbox );
					cell.Controls.Add( approveCheckbox );
					CheckBox deleteCheckbox = new CheckBox();
					deleteCheckbox.Text = "Delete";
					deleteCheckbox.ToolTip = "Delete the selected release.";
					deleteCheckbox.ID = string.Format( "ReleaseDelete.{0}.{1}", game.GameID, release.ReleaseID );
					deleteCheckbox.InputAttributes.Add( "group", "2" );
					_checkboxes.Add( deleteCheckbox );
					cell.Controls.Add( deleteCheckbox );
					row.Cells.Add( cell );
				}

				gamesTable.Rows.Add( row );
			}
		}
	}

	protected void ApproveAllButton_Click( object sender, EventArgs e )
	{
		UserSession session = UserSession.FromContext( this );
		if( session == null )
			throw new ApplicationException( "Unable to create session for user." );

		if( session.Database.IsAuthenticated == false )
			Response.Redirect( string.Format( "~/Error.aspx?errorId={0}", Errors.NotAuthenticated ), true );
		else if( session.Database.CanEdit == false )
			Response.Redirect( string.Format( "~/Error.aspx?errorId={0}", Errors.PermissionDenied ), true );
		else
		{
			switch( session.Database.ApproveAll() )
			{
				case UpdateResult.Succeeded:
					this.InfoLabel.Text = "All pending games and releases approved.";
					this.InfoLabel.Visible = true;
					break;
				case UpdateResult.Failed:
					Response.Redirect( string.Format( "~/Error.aspx?errorId={0}", Errors.ApplicationError ), true );
					return;
			}
		}
	}

	protected void ApproveSelectedButton_Click( object sender, EventArgs e )
	{
		UserSession session = UserSession.FromContext( this );
		if( session == null )
			throw new ApplicationException( "Unable to create session for user." );

		if( session.Database.IsAuthenticated == false )
			Response.Redirect( string.Format( "~/Error.aspx?errorId={0}", Errors.NotAuthenticated ), true );
		else if( session.Database.CanEdit == false )
			Response.Redirect( string.Format( "~/Error.aspx?errorId={0}", Errors.PermissionDenied ), true );
		else
		{
			bool someErrors = false;
			int approvedCount = 0;
			int deletedCount = 0;

			foreach( CheckBox checkbox in _checkboxes )
			{
				if( checkbox.Checked == false )
					continue;

				string id = checkbox.ID;
				string[] components = id.Split( '.' );
				if( ( components.Length == 0 ) || ( components.Length == 1 ) )
					continue;

				if( components[ 0 ] == "ReleaseApprove" )
				{
					long gameId;
					if( long.TryParse( components[ 1 ], out gameId ) == false )
						continue;
					long releaseId;
					if( long.TryParse( components[ 2 ], out releaseId ) == false )
						continue;

					switch( session.Database.ApproveGameRelease( gameId, releaseId ) )
					{
						case UpdateResult.Succeeded:
							approvedCount++;
							break;
						case UpdateResult.Failed:
						case UpdateResult.BadParameters:
							someErrors = true;
							break;
					}
				}
				else if( components[ 0 ] == "ReleaseDelete" )
				{
					long gameId;
					if( long.TryParse( components[ 1 ], out gameId ) == false )
						continue;
					long releaseId;
					if( long.TryParse( components[ 2 ], out releaseId ) == false )
						continue;

					switch( session.Database.RemoveGameRelease( gameId, releaseId ) )
					{
						case UpdateResult.Succeeded:
							deletedCount++;
							break;
						case UpdateResult.Failed:
						case UpdateResult.BadParameters:
							someErrors = true;
							break;
					}
				}
			}

			if( someErrors == false )
				this.InfoLabel.Text = string.Format( "{0} selected pending releases approved, {1} deleted.", approvedCount, deletedCount );
			else
				this.InfoLabel.Text = string.Format( "{0} selected pending releases approved, {1} deleted, but there were some errors.", approvedCount, deletedCount );
			this.InfoLabel.Visible = true;

			_checkboxes.Clear();
		}
	}
}
