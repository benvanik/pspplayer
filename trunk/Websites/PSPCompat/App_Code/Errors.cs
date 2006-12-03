// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Error helper.
/// </summary>
public static class Errors
{
	private static ErrorEntry[] _entries;
	private static Dictionary<int, ErrorEntry> _entryLookup;

	public const int ApplicationError = 0;

	public const int NotAuthenticated = 1000;
	public const int InvalidCredentials = 1001;
	public const int AccountDisabled = 1002;
	public const int GenericAuthenticationError = 1003;
	public const int PermissionDenied = 1004;

	static Errors()
	{
		_entries = new ErrorEntry[]{
			new ErrorEntry( ApplicationError, "An application error occured.", "An unknown error occured while processing your request.", "" ),

			new ErrorEntry( NotAuthenticated, "Not authenticated.", "You are not currently logged in. Please log in and try again.", "" ),
			new ErrorEntry( InvalidCredentials, "Invalid username or password.", "The username you entered was not found or the password did not match the one on file.", "" ),
			new ErrorEntry( AccountDisabled, "Account not active.", "The given account has not yet been confirmed or is disabled.", "" ),
			new ErrorEntry( GenericAuthenticationError, "Authentication error.", "Your account could not be authenticated.", "" ),
			new ErrorEntry( PermissionDenied, "Permission Denied.", "Your account does not have permission to perform this operation.", "" ),
		};

		_entryLookup = new Dictionary<int, ErrorEntry>( _entries.Length );
		foreach( ErrorEntry entry in _entries )
			_entryLookup.Add( entry.ErrorID, entry );
	}

	public static ErrorEntry Lookup( int errorId )
	{
		if( _entryLookup.ContainsKey( errorId ) == false )
			return null;
		else
			return _entryLookup[ errorId ];
	}
}

public class ErrorEntry
{
	public int ErrorID;
	public string Message;
	public string Description;
	public string HelpURL;
	public ErrorCause[] Causes;

	public ErrorEntry( int errorId, string message, string description, string helpUrl )
	{
		this.ErrorID = errorId;
		this.Message = message;
		this.Description = description;
		this.HelpURL = helpUrl;
	}

	public ErrorEntry( int errorId, string message, string description, string helpUrl, ErrorCause cause )
		: this( errorId, message, description, helpUrl, new ErrorCause[] { cause } )
	{
	}

	public ErrorEntry( int errorId, string message, string description, string helpUrl, ErrorCause[] causes )
		: this( errorId, message, description, helpUrl )
	{
		this.Causes = causes;
	}
}

public class ErrorCause
{
	public string Description;
	public string HelpURL;

	public ErrorCause( string description, string helpUrl )
	{
		this.Description = description;
		this.HelpURL = helpUrl;
	}
}