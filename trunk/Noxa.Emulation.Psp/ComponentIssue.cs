// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Noxa.Emulation.Psp
{
	public enum IssueLevel
	{
		Warning,
		Error,
	}

	public class ComponentIssue
	{
		private IComponent _component;
		private IssueLevel _level;
		private string _message;
		private string _supportUrl;

		public ComponentIssue( IComponent component, IssueLevel level, string message )
			: this( component, level, message, null )
		{
		}

		public ComponentIssue( IComponent component, IssueLevel level, string message, string supportUrl )
		{
			Debug.Assert( message != null );

			_component = component;
			_level = level;
			_message = message;
			_supportUrl = supportUrl;
		}

		public IssueLevel Level
		{
			get
			{
				return _level;
			}
		}

		public string Message
		{
			get
			{
				return _message;
			}
		}

		public string SupportUrl
		{
			get
			{
				return _supportUrl;
			}
		}

		public override string ToString()
		{
			string level;
			switch( _level )
			{
				case IssueLevel.Error:
					level = "Error";
					break;
				case IssueLevel.Warning:
					level = "Warning";
					break;
				default:
					level = "Unknown";
					Debug.Assert( false );
					break;
			}
			return string.Format( "[{0}] {1}", level, _message );
		}
	}
}
