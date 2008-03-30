// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2008 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Text;
using System.Windows.Forms;
using Noxa.Emulation.Psp.Player.Debugger.Model;

namespace Noxa.Emulation.Psp.Player.Debugger.Dialogs
{
	partial class JumpToMethodDialog : Form
	{
		public readonly InprocDebugger Debugger;
		public MethodBody Method;
		private int _codeVersion = -1;

		public JumpToMethodDialog()
		{
			this.InitializeComponent();

			this.DialogResult = DialogResult.Cancel;
		}

		public JumpToMethodDialog( InprocDebugger debugger )
			: this()
		{
			this.Debugger = debugger;
		}

		protected override void OnShown( EventArgs e )
		{
			base.OnShown( e );

			if( _codeVersion != this.Debugger.CodeCache.Version )
			{
				this.methodTextBox.AutoCompleteCustomSource.Clear();
				List<string> methodNames = new List<string>( this.Debugger.CodeCache.Methods.Count );
				foreach( MethodBody method in this.Debugger.CodeCache.Methods )
					methodNames.Add( method.Name );
				this.methodTextBox.AutoCompleteCustomSource.AddRange( methodNames.ToArray() );
				_codeVersion = this.Debugger.CodeCache.Version;
			}
			if( this.Method != null )
				this.methodTextBox.Text = this.Method.Name;
			this.methodTextBox.Focus();
			this.methodTextBox.SelectAll();
		}

		private void jumpButton_Click( object sender, EventArgs e )
		{
			this.Method = null;
			string name = this.methodTextBox.Text.Trim();
			foreach( MethodBody method in this.Debugger.CodeCache.Methods )
			{
				if( method.Name == name )
				{
					this.Method = method;
					break;
				}
			}
			if( this.Method == null )
			{
				SystemSounds.Exclamation.Play();
				this.methodTextBox.Focus();
				this.methodTextBox.SelectAll();
				return;
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click( object sender, EventArgs e )
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void JumpToMethodDialog_Activated( object sender, EventArgs e )
		{
			this.methodTextBox.Focus();
			this.methodTextBox.SelectAll();
		}
	}
}
