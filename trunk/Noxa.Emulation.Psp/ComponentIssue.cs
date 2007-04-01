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
	/// <summary>
	/// Defines the severity level of a <see cref="ComponentIssue"/>.
	/// </summary>
	public enum IssueLevel
	{
		/// <summary>
		/// User can proceed, but things may not function properly.
		/// </summary>
		Warning,

		/// <summary>
		/// User cannot proceed.
		/// </summary>
		Error,
	}

	/// <summary>
	/// Represents an issue that occured during emulation setup.
	/// </summary>
	public class ComponentIssue
	{
		/// <summary>
		/// The <see cref="IComponent"/> instance the issue relates to.
		/// </summary>
		public readonly IComponent Component;

		/// <summary>
		/// The severity level of the issue.
		/// </summary>
		public readonly IssueLevel Level;

		/// <summary>
		/// The human-friendly message describing the issue.
		/// </summary>
		public readonly string Message;

		/// <summary>
		/// A URL containing support information.
		/// </summary>
		public readonly string SupportUrl;

		/// <summary>
		/// Initializes a new <see cref="ComponentIssue"/> instance with the given parameters.
		/// </summary>
		/// <param name="component">The <see cref="IComponent"/> instance the issue relates to.</param>
		/// <param name="level">The severity level of the issue.</param>
		/// <param name="message">A message describing the issue.</param>
		public ComponentIssue( IComponent component, IssueLevel level, string message )
			: this( component, level, message, null )
		{
		}

		/// <summary>
		/// Initializes a new <see cref="ComponentIssue"/> instance with the given parameters.
		/// </summary>
		/// <param name="component">The <see cref="IComponent"/> instance the issue relates to.</param>
		/// <param name="level">The severity level of the issue.</param>
		/// <param name="message">A message describing the issue.</param>
		/// <param name="supportUrl">A URL containing support information.</param>
		public ComponentIssue( IComponent component, IssueLevel level, string message, string supportUrl )
		{
			Debug.Assert( message != null );

			this.Component = component;
			this.Level = level;
			this.Message = message;
			this.SupportUrl = supportUrl;
		}

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current <see cref="ComponentIssue"/>.
		/// </summary>
		public override string ToString()
		{
			string level;
			switch( this.Level )
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
			return string.Format( "[{0}] {1}", level, this.Message );
		}
	}
}
