// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

/// <summary>
/// User session collection helper.
/// </summary>
public class UserSession
{
	protected Database _database;

	public UserSession()
	{
		_database = new Database();
	}

	public Database Database
	{
		get
		{
			return _database;
		}
	}

	public static UserSession FromContext( UserControl control )
	{
		if( control == null )
			throw new ArgumentNullException( "control" );
		return FromContext( control.Session );
	}

	public static UserSession FromContext( Page control )
	{
		if( control == null )
			throw new ArgumentNullException( "page" );
		return FromContext( control.Session );
	}

	public static UserSession FromContext( HttpSessionState session )
	{
		return FromContext( session, false );
	}

	public static UserSession FromContext( HttpSessionState session, bool failOnMissing )
	{
		if( session == null )
			throw new ArgumentNullException( "session" );

		UserSession mySession = session[ "UserSession" ] as UserSession;
		if( ( mySession == null ) &&
			( failOnMissing == false ) )
		{
			mySession = new UserSession();
			session.Add( "UserSession", mySession );
		}
		return mySession;
	}
}
