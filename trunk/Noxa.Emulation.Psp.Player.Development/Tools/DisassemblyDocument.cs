// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Puzzle.SourceCode;

using Noxa.Utilities.Controls;

using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Player.Development.Tools
{
	partial class DisassemblyDocument : Noxa.Emulation.Psp.Player.Development.Tools.ToolDocument
	{
		private Studio _studio;

		private int _statementIcon;
		private int _statementCallIcon;
		private int _statementDeadIcon;

		private Dictionary<int, Statement> _statements;

		private Method _currentMethod;
		private Dictionary<int, Row> _rowMapping;		// Address->row

		public DisassemblyDocument()
		{
			InitializeComponent();
		}

		public DisassemblyDocument( Studio studio )
			: this()
		{
			Debug.Assert( studio != null );
			if( studio == null )
				throw new ArgumentNullException( "studio" );
			_studio = studio;

			this.Icon = IconUtilities.ConvertToIcon( Properties.Resources.DisassemblyIcon );

			_statementIcon = codeEditorControl.GutterIcons.Images.Add( Properties.Resources.StatementIcon, Color.Transparent );
			_statementCallIcon = codeEditorControl.GutterIcons.Images.Add( Properties.Resources.StatementCallIcon, Color.Transparent );
			_statementDeadIcon = codeEditorControl.GutterIcons.Images.Add( Properties.Resources.StatementDeadIcon, Color.Transparent );

			_statements = new Dictionary<int, Statement>();

			this.PopulateMethods();

			//CodeEditorSyntaxLoader.SetSyntax( _editor, SyntaxLanguage.Text );
		}

		private void PopulateMethods()
		{
			methodsToolStripComboBox.BeginUpdate();
			methodsToolStripComboBox.Items.Clear();
			foreach( Method method in _studio.Debugger.DebugData.Methods )
				methodsToolStripComboBox.Items.Add( method );
			methodsToolStripComboBox.EndUpdate();
		}

		public Method CurrentMethod
		{
			get
			{
				return _currentMethod;
			}
		}

		private void methodsToolStripComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			Method method = methodsToolStripComboBox.SelectedItem as Method;
			if( method == null )
				return;
			this.DisplayMethod( method );
		}

		public void DisplayMethod( Method method )
		{
			if( _currentMethod == method )
				return;

			_currentMethod = method;
			_rowMapping = new Dictionary<int, Row>( method.Instructions.Count );

			if( methodsToolStripComboBox.Items.Contains( method ) == true )
				methodsToolStripComboBox.SelectedItem = method;

			syntaxDocument.Clear();

			syntaxDocument.Text = string.Format( "{0:X8} <{1}>:", method.EntryAddress, method.Name );

			foreach( Instruction instr in method.Instructions.Values )
			{
				Row row = syntaxDocument.Add( string.Format( " {0:X8}:  {1:X8}    {2} {3}", instr.Address, instr.Code, instr.Opcode, instr.Operands ), false );
				_rowMapping.Add( instr.Address, row );
			}

			syntaxDocument.UndoBuffer.Clear();

			foreach( Statement statement in _statements.Values )
			{
				this.SetStatement( statement );
			}
		}

		#region Statements

		public void SetStatement( Statement statement )
		{
			if( statement.Method != _currentMethod )
				return;

			Debug.Assert( _rowMapping.ContainsKey( statement.Address ) == true );
			if( _rowMapping.ContainsKey( statement.Address ) == false )
				return;
			Row row = _rowMapping[ statement.Address ];

			int icon;
			switch( statement.Type )
			{
				default:
				case StatementType.Current:
					icon = _statementIcon;
					break;
				case StatementType.Call:
					icon = _statementCallIcon;
					break;
				case StatementType.Dead:
					icon = _statementDeadIcon;
					break;
			}
			row.Images.Add( icon );
		}

		public Statement AddStatement( StatementType type, int address )
		{
			Method method = _studio.Debugger.DebugData.FindMethod( address );
			if( method == null )
			{
				// TODO: support statements in methods we don't have, for when we debug crazy stuff
				return null;
			}
			return this.AddStatement( type, method, address );
		}

		public Statement AddStatement( StatementType type, Method method, int address )
		{
			Statement statement = new Statement( type, method, address );
			_statements.Add( statement.Address, statement );

			this.SetStatement( statement );

			return statement;
		}

		public void RemoveStatement( int address )
		{
			if( _statements.ContainsKey( address ) == false )
				return;
			this.RemoveStatement( _statements[ address ] );
		}

		public void RemoveStatement( Statement statement )
		{
			Debug.Assert( statement != null );
			if( statement == null )
				throw new ArgumentNullException( "statement" );

			if( statement.Method != _currentMethod )
				return;

			if( _rowMapping.ContainsKey( statement.Address ) == false )
				return;
			Row row = _rowMapping[ statement.Address ];
			switch( statement.Type )
			{
				case StatementType.Current:
					if( row.Images.Contains( _statementIcon ) == true )
						row.Images.Remove( _statementIcon );
					break;
				case StatementType.Call:
					if( row.Images.Contains( _statementCallIcon ) == true )
						row.Images.Remove( _statementCallIcon );
					break;
				case StatementType.Dead:
					if( row.Images.Contains( _statementDeadIcon ) == true )
						row.Images.Remove( _statementDeadIcon );
					break;
			}

			_statements.Remove( statement.Address );
		}

		public void ClearAllStatements()
		{
			List<Statement> statements = new List<Statement>( _statements.Values );
			foreach( Statement statement in statements )
				this.RemoveStatement( statement );

			_statements.Clear();
		}

		#endregion
	}

	enum StatementType
	{
		Current,
		Call,
		Dead,
	}

	class Statement
	{
		public StatementType Type;
		public Method Method;
		public int Address;

		public Statement( StatementType type, Method method, int address )
		{
			this.Type = type;
			this.Method = method;
			this.Address = address;
		}
	}
}

