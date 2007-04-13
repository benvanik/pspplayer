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
using System.Diagnostics;

public partial class Error : System.Web.UI.Page
{
	protected void Page_Load( object sender, EventArgs e )
	{
		int errorId;
		if( int.TryParse( Request.QueryString[ "errorId" ], out errorId ) == false )
		{
			Debug.Assert( false, "No ErrorID passed to the error page." );
			errorId = Errors.ApplicationError;
		}

		ErrorEntry entry = Errors.Lookup( errorId );
		if( entry == null )
			entry = new ErrorEntry( 0, "Fallback error.", "Something has gone so wrong that I couldn't retreive the error information for you.", "" );

		this.MessageLabel.Text = string.Format( "{0} [{1}]", entry.Message, errorId );
		this.DescriptionLabel.Text = entry.Description;
		if( ( entry.HelpURL == null ) ||
			( entry.HelpURL == string.Empty ) )
			this.MoreInformationLabel.Text = "None";
		else
			this.MoreInformationLabel.Text = string.Format( "<a href=\"{0}\">Support Site</a>", entry.HelpURL );

		// TODO: Write out to the cause table
	}
}
