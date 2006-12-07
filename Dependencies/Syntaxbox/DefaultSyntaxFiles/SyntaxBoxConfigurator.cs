using System;
using System.IO;
using System.Reflection;
using Puzzle.SourceCode;
using Puzzle.Windows.Forms;

namespace Puzzle.Syntaxbox.DefaultSyntaxFiles
{
	/// <summary>
	/// Summary description for SyntaxBoxConfigurator.
	/// </summary>
	public class SyntaxBoxConfigurator
	{
		public SyntaxBoxConfigurator(SyntaxBoxControl syntaxBoxControl)
		{
			this.syntaxBoxControl = syntaxBoxControl;
		}

		#region Property  SyntaxBoxControl
		
		private SyntaxBoxControl syntaxBoxControl;
		
		public SyntaxBoxControl SyntaxBoxControl
		{
			get { return this.syntaxBoxControl; }
			set { this.syntaxBoxControl = value; }
		}
		
		#endregion

		private void SetupSyntax(string fileName)
		{
			Assembly asm = this.GetType().Assembly;
			string syntax = ReadAssemblyFile(asm, fileName); 
			LoadSyntaxString(syntax);
		}
		
		public void LoadSyntaxString(string syntaxString)
		{
			syntaxBoxControl.Document.Parser.Init(Language.FromSyntaxXml(syntaxString));				
		}

		public void SetupNPath()
		{
			string fileName = @"Puzzle.SyntaxBox.DefaultSyntaxFiles.npath.syn";
			SetupSyntax(fileName);
		}


		public void SetupCSharp()
		{
			string fileName = @"Puzzle.SyntaxBox.DefaultSyntaxFiles.CSharp.syn";
			SetupSyntax(fileName);
		}

		public void SetupVBNet()
		{
			string fileName = @"Puzzle.SyntaxBox.DefaultSyntaxFiles.VB.NET.syn";
			SetupSyntax(fileName);
		}

		public void SetupDelphi()
		{
			string fileName = @"Puzzle.SyntaxBox.DefaultSyntaxFiles.Delphi.syn";
			SetupSyntax(fileName);
		}

		public void SetupXml()
		{
			string fileName = @"Puzzle.SyntaxBox.DefaultSyntaxFiles.XML.syn";
			SetupSyntax(fileName);
		}

		public void SetupText()
		{
			string fileName = @"Puzzle.SyntaxBox.DefaultSyntaxFiles.Text.syn";
			SetupSyntax(fileName);
		}

		public void SetupSqlServer2KSql()
		{
			string fileName = @"Puzzle.SyntaxBox.DefaultSyntaxFiles.SQLServer2K_SQL.syn";
			SetupSyntax(fileName);
		}

		public string ReadAssemblyFile(Assembly asm, string name)
		{
			string result = "";
			StreamReader reader = null;
			using (Stream stream = asm.GetManifestResourceStream(name))
			{
				if (stream == null)
				{
					string errorMessage = string.Format("Assembly resource '{0}' not found",name);
					throw new Exception(errorMessage);
				}
				else
				{

					try
					{
						reader = new StreamReader(stream);
						result = reader.ReadToEnd(); 			
						reader.Close() ;
						reader = null;					
					}
					catch (Exception ex)
					{
						if (reader != null)
						{
							try
							{
								reader.Close();
							}
							catch
							{
							}
							throw new IOException("Could not load file from embedded resource: " + name + "! " + ex.Message, ex);
						}
					}
				}
			}			
			return result;
		}


	}
}
