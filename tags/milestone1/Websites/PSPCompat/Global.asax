<%@ Application Language="C#" %>
<%@ Import Namespace="System.Diagnostics" %>

<script runat="server">
// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
		Exception ex = Server.GetLastError().GetBaseException();
		
		if( ex != null )
			Debug.Assert( false, ex.ToString() );

		int errorId = Errors.ApplicationError;
		if( ex is NotAuthenticatedException )
			errorId = Errors.NotAuthenticated;
		else if( ex is PermissionDeniedException )
			errorId = Errors.PermissionDenied;

		Response.Redirect( string.Format( "~/Error.aspx?errorId={0}", errorId ), true );
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
