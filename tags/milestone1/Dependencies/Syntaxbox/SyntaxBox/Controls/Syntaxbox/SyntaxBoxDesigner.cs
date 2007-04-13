// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using Puzzle.SourceCode;

namespace Puzzle.Windows.Forms.SyntaxBox
{
	/// <summary>
	/// Designer for the SyntaxBoxControl
	/// </summary>
	public class SyntaxBoxDesigner : ControlDesigner
	{
		public SyntaxBoxDesigner() : base()
		{
		}

		protected ISelectionService SelectionService
		{
			get { return (ISelectionService) this.GetService(typeof (ISelectionService)); }
		}

		protected void OnActivate(object s, EventArgs e)
		{
		}

		protected virtual IDesignerHost DesignerHost
		{
			get { return (IDesignerHost) base.GetService(typeof (IDesignerHost)); }
		}

		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();
			if (DesignerHost != null)
			{
				DesignerTransaction trans = DesignerHost.CreateTransaction(
					"Adding Syntaxdocument");
				SyntaxDocument sd = DesignerHost.CreateComponent
					(typeof (SyntaxDocument)) as
					SyntaxDocument;

				SyntaxBoxControl sb = this.Control as SyntaxBoxControl;
				sb.Document = sd;
				trans.Commit();
			}
		}
	}
}