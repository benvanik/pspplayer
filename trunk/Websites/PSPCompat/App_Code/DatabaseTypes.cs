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
