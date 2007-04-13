// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class Game
{
	public long GameID;
	public string Title;
	public string Website;
	public List<GameRelease> Releases;
}

public class GameRelease
{
	public long GameID;
	public long ReleaseID;
	public int? Region;
	public string DiscID;
	public string Title;
	public string Website;
	public DateTime? ReleaseDate;
	public float? GameVersion;
	public float? SystemVersion;
	public bool HasIcon;
}

public enum Region
{
	Unknown = 0,
	America = 1,
	Europe = 2,
	Japan = 32768,
	Australia = 4,
	SouthKorea = 5,
}

public enum SecurityLevel
{
	Banned = -1,
	Unknown = 0,
	Viewer = 1,
	Editor = 2,
	Administrator = 3,
}