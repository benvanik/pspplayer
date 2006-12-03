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
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		UserSession s = UserSession.FromContext( this );

		//byte[] iconData = File.ReadAllBytes( this.Server.MapPath( "Resources/Test Game Icons/0.png" ) );
		//s.Database.SetGameIcon( 307, 910, iconData );

		//iconData = File.ReadAllBytes( this.Server.MapPath( "Resources/Test Game Icons/1.png" ) );
		//s.Database.SetGameIcon( 307, 911, iconData );

		//iconData = File.ReadAllBytes( this.Server.MapPath( "Resources/Test Game Icons/2.png" ) );
		//s.Database.SetGameIcon( 307, 912, iconData );
    }
}
