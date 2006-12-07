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

using Noxa.Emulation.Psp.Debugging;

namespace Noxa.Emulation.Psp.Player.Development.Tools
{
	partial class DisassemblyDocument : Noxa.Emulation.Psp.Player.Development.Tools.ToolDocument
	{
		private Studio _studio;

		private int _statementIcon;
		private int _statementCallIcon;
		private int _statementDeadIcon;

		private Method _currentMethod;

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

			_statementIcon = codeEditorControl.GutterIcons.Images.Add( Properties.Resources.StatementIcon, Color.Transparent );
			_statementCallIcon = codeEditorControl.GutterIcons.Images.Add( Properties.Resources.StatementCallIcon, Color.Transparent );
			_statementDeadIcon = codeEditorControl.GutterIcons.Images.Add( Properties.Resources.StatementDeadIcon, Color.Transparent );

			//CodeEditorSyntaxLoader.SetSyntax( _editor, SyntaxLanguage.Text );
		}

		public Method CurrentMethod
		{
			get
			{
				return _currentMethod;
			}
		}

		public void DisplayMethod( Method method )
		{
			syntaxDocument.Clear();

			syntaxDocument.Text = string.Format( "{0:X8} <{1}>:", method.EntryAddress, method.Name );

			foreach( Instruction instr in method.Instructions.Values )
			{
				syntaxDocument.Add( string.Format( " {0:X8}:  {1:X8}    {2} {3}", instr.Address, instr.Code, instr.Opcode, instr.Operands ), false );
			}

			syntaxDocument.UndoBuffer.Clear();
		}
	}
}

