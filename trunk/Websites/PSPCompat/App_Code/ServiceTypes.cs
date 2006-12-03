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

public enum AddResult
{
	Succeeded = 0,
	PermissionDenied = 1,
	Redundant = 2,
	Failed = 3,
}

public enum CompatibilityLevel
{
	Unknown = 0,
	Crashing = 1,
	Menus = 2,
	Playable = 3,
	Complete = 4,
}

public class CompatibilityResults
{
	public bool Found;

	public long GameID;
	public long ReleaseID;

	public Game Game;

	public CompatibilityLevel Level;

	public string[] Notes;
}

public enum ComponentInfoType
{
	Audio = 0,
	Bios = 1,
	Cpu = 2,
	Input = 3,
	UserMedia = 4,
	GameMedia = 5,
	Network = 6,
	Video = 7,
	Other = 9
}

public enum ComponentInfoBuild
{
	Debug = 0,
	Testing = 1,
	Release = 2
}

public class ComponentInfo
{
	public string Filename;
	public ComponentInfoType Type;
	public string Name;
	public Version Version;
	public string Website;
	public ComponentInfoBuild Build;
}