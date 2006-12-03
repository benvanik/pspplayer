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
using System.Drawing;

public partial class AuthenticationControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
		UserSession session = UserSession.FromContext( this );
		if( session == null )
			throw new ApplicationException( "Unable to create session for user." );

		if( session.Database.IsAuthenticated == false )
		{
			this.PreAuthPanel.Visible = true;
			this.PostAuthPanel.Visible = false;
		}
		else
		{
			this.PreAuthPanel.Visible = false;
			this.PostAuthPanel.Visible = true;

			this.UserInfoLabel.Text = session.Database.Username;
			this.UserInfoLabel.Style.Add( "font-weight", "bold" );
			switch( session.Database.SecurityLevel )
			{
				default:
				case SecurityLevel.Viewer:
					break;
				case SecurityLevel.Editor:
					this.UserInfoLabel.Style.Add( "font-color", string.Format( "#{0:X8}", Color.LightCoral.ToArgb() ) );
					break;
				case SecurityLevel.Administrator:
					this.UserInfoLabel.Style.Add( "font-color", string.Format( "#{0:X8}", Color.DarkBlue.ToArgb() ) );
					break;
			}
		}
    }

	protected void LoginButton_Click( object sender, ImageClickEventArgs e )
	{
		UserSession session = UserSession.FromContext( this );
		if( session == null )
			throw new ApplicationException( "Unable to create session for user." );

		string username = this.UsernameTextBox.Text.Trim();
		string password = this.PasswordTextBox.Text.Trim();

		if( ( username.Length == 0 ) || ( password.Length == 0 ) )
		{
			Response.Redirect( string.Format( "~/Error.aspx?errorId={0}", Errors.InvalidCredentials ), true );
			return;
		}

		if( session.Database.IsAuthenticated == true )
			session.Database.Logout();

		AuthenticationResult result = session.Database.Authenticate( username, password );
		switch( result )
		{
			case AuthenticationResult.Succeeded:
				Response.Redirect( "~/", true );
				break;
			case AuthenticationResult.InvalidCredentials:
				Response.Redirect( string.Format( "~/Error.aspx?errorId={0}", Errors.InvalidCredentials ), true );
				break;
			case AuthenticationResult.AccountDisabled:
				Response.Redirect( string.Format( "~/Error.aspx?errorId={0}", Errors.AccountDisabled ), true );
				break;
			default:
				Response.Redirect( string.Format( "~/Error.aspx?errorId={0}", Errors.GenericAuthenticationError ), true );
				break;
		}
	}

	protected void SignOutButton_Click( object sender, EventArgs e )
	{
		UserSession session = UserSession.FromContext( this );
		if( session == null )
		{
			// This is ok - shouldn't present an error if the user wants to leave
			//throw new ApplicationException( "Unable to create session for user." );
			Response.Redirect( "~/", true );
			return;
		}

		if( session.Database.IsAuthenticated == false )
			return;

		session.Database.Logout();

		Response.Redirect( "~/", true );
	}
}
