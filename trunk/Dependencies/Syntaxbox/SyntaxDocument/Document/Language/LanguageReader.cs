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
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Xml;

namespace Puzzle.SourceCode
{
	/// <summary>
	/// 
	/// </summary>
	public class SyntaxLoader
	{
		private Hashtable mStyles = new Hashtable();
		private Hashtable mBlocks = new Hashtable();
		private Language mLanguage = new Language();
		//protected BlockType		mLanguage.MainBlock=null;


		/// <summary>
		/// Load a specific language file
		/// </summary>
		/// <param name="File">File name</param>
		/// <returns>Language object</returns>
		public Language Load(string File)
		{
			mStyles = new Hashtable();
			mBlocks = new Hashtable();
			mLanguage = new Language();

			XmlDocument myXmlDocument = new XmlDocument();
			myXmlDocument.Load(File);
			ReadLanguageDef(myXmlDocument);

			return mLanguage;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="File"></param>
		/// <returns></returns>
		public Language Load(string File, string Separators)
		{
			mStyles = new Hashtable();
			mBlocks = new Hashtable();
			mLanguage = new Language();
			mLanguage.Separators = Separators;

			XmlDocument myXmlDocument = new XmlDocument();
			myXmlDocument.Load(File);
			ReadLanguageDef(myXmlDocument);

			if (mLanguage.MainBlock == null)
			{
				throw new Exception("no main block found in syntax");
			}

			return mLanguage;
		}

		/// <summary>
		/// Load a specific language from an xml string
		/// </summary>
		/// <param name="XML"></param>
		/// <returns></returns>
		public Language LoadXML(string XML)
		{
			mStyles = new Hashtable();
			mBlocks = new Hashtable();
			mLanguage = new Language();

			XmlDocument myXmlDocument = new XmlDocument();
			myXmlDocument.LoadXml(XML);
			ReadLanguageDef(myXmlDocument);

			if (mLanguage.MainBlock == null)
			{
				throw new Exception("no main block found in syntax");
			}


			return mLanguage;
		}

		private void ReadLanguageDef(XmlDocument xml)
		{
			ParseLanguage(xml["Language"]);
		}

		private void ParseLanguage(XmlNode node)
		{
			//get language name and startblock
			string Name = "";
			string StartBlock = "";

			foreach (XmlAttribute att in node.Attributes)
			{
				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "name")
					Name = att.Value;

				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "startblock")
					StartBlock = att.Value;
			}

			mLanguage.Name = Name;
			mLanguage.MainBlock = GetBlock(StartBlock);

			foreach (XmlNode n in node.ChildNodes)
			{
				if (n.NodeType == XmlNodeType.Element)
				{
					if (n.Name.ToLower(CultureInfo.InvariantCulture)
						== "filetypes")
						ParseFileTypes(n);
					if (n.Name.ToLower(CultureInfo.InvariantCulture)
						== "block")
						ParseBlock(n);
					if (n.Name.ToLower(CultureInfo.InvariantCulture)
						== "style")
						ParseStyle(n);
				}
			}
		}

		private void ParseFileTypes(XmlNode node)
		{
			foreach (XmlNode n in node.ChildNodes)
			{
				if (n.NodeType == XmlNodeType.Element)
				{
					if (n.Name.ToLower(CultureInfo.InvariantCulture)
						== "filetype")
					{
						//add filetype
						string Extension = "";
						string Name = "";
						foreach (XmlAttribute a in n.Attributes)
						{
							if (a.Name.ToLower
								(CultureInfo.InvariantCulture) == "name")
								Name = a.Value;
							if (a.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"extension")
								Extension = a.Value;
						}
						FileType ft = new FileType();
						ft.Extension = Extension;
						ft.Name = Name;
						mLanguage.FileTypes.Add(ft);
					}
				}
			}
		}

		private void ParseBlock(XmlNode node)
		{
			string Name = "", Style = "", PatternStyle = "";
			bool IsMultiline = false;
			bool TerminateChildren = false;
			Color BackColor = Color.Transparent;
			foreach (XmlAttribute att in node.Attributes)
			{
				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "name")
					Name = att.Value;
				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "style")
					Style = att.Value;
				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "patternstyle")
					PatternStyle = att.Value;
				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "ismultiline")
					IsMultiline = bool.Parse(att.Value);
				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "terminatechildren")
					TerminateChildren = bool.Parse(att.Value);
				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "backcolor")
				{
					BackColor = Color.FromName(att.Value);
					//Transparent =false;
				}
			}

			//create block object here
			BlockType bl = GetBlock(Name);
			bl.BackColor = BackColor;
			bl.Name = Name;
			bl.MultiLine = IsMultiline;
			bl.Style = GetStyle(Style);
			bl.TerminateChildren = TerminateChildren;
			//		if (PatternStyle!="")
			//			bl.PatternStyle = GetStyle(PatternStyle);
			//		else
			//			bl.PatternStyle = bl.Style;			

			foreach (XmlNode n in node.ChildNodes)
			{
				if (n.NodeType == XmlNodeType.Element)
				{
					if (n.Name.ToLower(CultureInfo.InvariantCulture)
						== "scope")
					{
						//bool IsComplex=false;
						//bool IsSeparator=false;
						string Start = "";
						string End = "";
						string style = "";
						string text = "";
						string EndIsSeparator = "";
						string StartIsSeparator = "";
						string StartIsComplex = "false";
						string EndIsComplex = "false";
						string StartIsKeyword = "false";
						string EndIsKeyword = "false";
						string spawnstart = "";
						string spawnend = "";
						string EscapeChar = "";
						string CauseIndent = "false";

						bool expanded = true;

						foreach (XmlAttribute att in n.Attributes)
						{
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"start")
								Start = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"escapechar")
								EscapeChar = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) == "end")
								End = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"style")
								style = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) == "text")
								text = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"defaultexpanded")
								expanded = bool.Parse(att.Value);
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"endisseparator")
								EndIsSeparator = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"startisseparator")
								StartIsSeparator = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"startiskeyword")
								StartIsKeyword = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"startiscomplex")
								StartIsComplex = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"endiscomplex")
								EndIsComplex = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"endiskeyword")
								EndIsKeyword = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"spawnblockonstart")
								spawnstart = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"spawnblockonend")
								spawnend = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"causeindent")
								CauseIndent = att.Value;
						}
						if (Start != "")
						{
							//bl.StartPattern =new Pattern (Pattern,IsComplex,false,IsSeparator);
							//bl.StartPatterns.Add (new Pattern (Pattern,IsComplex,IsSeparator,true));
							Scope scop = new Scope();
							scop.Style = GetStyle(style);
							scop.ExpansionText = text;
							scop.DefaultExpanded = expanded;
							bool blnStartIsComplex = bool.Parse(StartIsComplex);
							bool blnEndIsComplex = bool.Parse(EndIsComplex);
							bool blnCauseIndent = bool.Parse(CauseIndent);
							scop.CauseIndent = blnCauseIndent;

							Pattern StartP = new Pattern(Start, blnStartIsComplex, false,
							                             bool.Parse(StartIsKeyword));
							Pattern EndP = null;
							if (EscapeChar != "")
							{
								EndP = new Pattern(End, blnEndIsComplex, false, bool.Parse
									(EndIsKeyword), EscapeChar);
							}
							else
							{
								EndP = new Pattern(End, blnEndIsComplex, false, bool.Parse
									(EndIsKeyword));
							}

							if (EndIsSeparator != "")
								EndP.IsSeparator = bool.Parse(EndIsSeparator);
							scop.Start = StartP;
							scop.EndPatterns.Add(EndP);
							bl.ScopePatterns.Add(scop);
							if (spawnstart != "")
							{
								scop.SpawnBlockOnStart = GetBlock(spawnstart);
							}
							if (spawnend != "")
							{
								scop.SpawnBlockOnEnd = GetBlock(spawnend);
							}
						}
					}
					if (n.Name.ToLower(CultureInfo.InvariantCulture)
						== "bracket")
					{
						//bool IsComplex=false;
						//bool IsSeparator=false;
						string Start = "";
						string End = "";
						string style = "";

						string EndIsSeparator = "";
						string StartIsSeparator = "";

						string StartIsComplex = "false";
						string EndIsComplex = "false";

						string StartIsKeyword = "false";
						string EndIsKeyword = "false";
						string IsMultiLineB = "true";

						foreach (XmlAttribute att in n.Attributes)
						{
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"start")
								Start = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) == "end")
								End = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"style")
								style = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"endisseparator")
								EndIsSeparator = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"startisseparator")
								StartIsSeparator = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"startiskeyword")
								StartIsKeyword = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"startiscomplex")
								StartIsComplex = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"endiscomplex")
								EndIsComplex = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"endiskeyword")
								EndIsKeyword = att.Value;
							if (att.Name.ToLower
								(CultureInfo.InvariantCulture) ==
								"ismultiline")
								IsMultiLineB = att.Value;
						}
						if (Start != "")
						{
							PatternList pl = new PatternList();
							pl.Style = GetStyle(style);

							bool blnStartIsComplex = bool.Parse(StartIsComplex);
							bool blnEndIsComplex = bool.Parse(EndIsComplex);
							bool blnIsMultiLineB = bool.Parse(IsMultiLineB);

							Pattern StartP = new Pattern(Start, blnStartIsComplex, false,
							                             bool.Parse(StartIsKeyword));
							Pattern EndP = new Pattern(End, blnEndIsComplex, false,
							                           bool.Parse(EndIsKeyword));

							StartP.MatchingBracket = EndP;
							EndP.MatchingBracket = StartP;
							StartP.BracketType = BracketType.StartBracket;
							EndP.BracketType = BracketType.EndBracket;
							StartP.IsMultiLineBracket = EndP.IsMultiLineBracket =
								blnIsMultiLineB;

							pl.Add(StartP);
							pl.Add(EndP);
							bl.OperatorsList.Add(pl);
						}
					}

				}

				if (n.Name.ToLower(CultureInfo.InvariantCulture)
					== "keywords")
					foreach (XmlNode cn in n.ChildNodes)
					{
						if (cn.Name.ToLower(CultureInfo.InvariantCulture)
							== "patterngroup")
						{
							PatternList pl = new PatternList();
							bl.KeywordsList.Add(pl);
							foreach (XmlAttribute att in cn.Attributes)
							{
								if (att.Name.ToLower
									(CultureInfo.InvariantCulture) ==
									"style")
									pl.Style = GetStyle(att.Value);

								if (att.Name.ToLower
									(CultureInfo.InvariantCulture) == "name")
									pl.Name = att.Value;

								if (att.Name.ToLower
									(CultureInfo.InvariantCulture) ==
									"normalizecase")
									pl.NormalizeCase = bool.Parse(att.Value);

								if (att.Name.ToLower
									(CultureInfo.InvariantCulture) ==
									"casesensitive")
									pl.CaseSensitive = bool.Parse(att.Value);

							}
							foreach (XmlNode pt in cn.ChildNodes)
							{
								if (pt.Name.ToLower
									(CultureInfo.InvariantCulture) ==
									"pattern")
								{
									bool IsComplex = false;
									bool IsSeparator = false;
									string Category = null;
									string Pattern = "";
									if (pt.Attributes != null)
									{
										foreach (XmlAttribute att in pt.Attributes)
										{
											if (att.Name.ToLower
												(CultureInfo.InvariantCulture) ==
												"text")
												Pattern = att.Value;
											if (att.Name.ToLower
												(CultureInfo.InvariantCulture) ==
												"iscomplex")
												IsComplex = bool.Parse(att.Value);
											if (att.Name.ToLower
												(CultureInfo.InvariantCulture) ==
												"isseparator")
												IsSeparator = bool.Parse(att.Value);
											if (att.Name.ToLower
												(CultureInfo.InvariantCulture) ==
												"category")
												Category = (att.Value);

										}
									}
									if (Pattern != "")
									{
										Pattern pat = new Pattern(Pattern, IsComplex, IsSeparator,
										                          true);
										pat.Category = Category;
										pl.Add(pat);
									}

								}
								else if (pt.Name.ToLower
									(CultureInfo.InvariantCulture) ==
									"patterns")
								{
									string Patterns = pt.ChildNodes[0].Value;
									Patterns = Patterns.Replace("\t", " ");
									while (Patterns.IndexOf("  ") >= 0)
										Patterns = Patterns.Replace("  ", " ");


									foreach (string Pattern in Patterns.Split())
									{
										if (Pattern != "")
											pl.Add(new Pattern(Pattern, false, false, true));
									}
								}
							}
						}
					}
				//if (n.Name == "Operators")
				//	ParseStyle(n);
				if (n.Name.ToLower(CultureInfo.InvariantCulture)
					== "operators")
					foreach (XmlNode cn in n.ChildNodes)
					{
						if (cn.Name.ToLower(CultureInfo.InvariantCulture)
							== "patterngroup")
						{
							PatternList pl = new PatternList();
							bl.OperatorsList.Add(pl);
							foreach (XmlAttribute att in cn.Attributes)
							{
								if (att.Name.ToLower
									(CultureInfo.InvariantCulture) ==
									"style")
									pl.Style = GetStyle(att.Value);

								if (att.Name.ToLower
									(CultureInfo.InvariantCulture) == "name")
									pl.Name = att.Value;

								if (att.Name.ToLower
									(CultureInfo.InvariantCulture) ==
									"normalizecase")
									pl.NormalizeCase = bool.Parse(att.Value);

								if (att.Name.ToLower
									(CultureInfo.InvariantCulture) ==
									"casesensitive")
									pl.CaseSensitive = bool.Parse(att.Value);
							}

							foreach (XmlNode pt in cn.ChildNodes)
							{
								if (pt.Name.ToLower
									(CultureInfo.InvariantCulture) ==
									"pattern")
								{
									bool IsComplex = false;
									bool IsSeparator = false;
									string Pattern = "";
									string Category = null;
									if (pt.Attributes != null)
									{
										foreach (XmlAttribute att in pt.Attributes)
										{
											if (att.Name.ToLower
												(CultureInfo.InvariantCulture) ==
												"text")
												Pattern = att.Value;
											if (att.Name.ToLower
												(CultureInfo.InvariantCulture) ==
												"iscomplex")
												IsComplex = bool.Parse(att.Value);
											if (att.Name.ToLower
												(CultureInfo.InvariantCulture) ==
												"isseparator")
												IsSeparator = bool.Parse(att.Value);
											if (att.Name.ToLower
												(CultureInfo.InvariantCulture) ==
												"category")
												Category = (att.Value);

										}
									}
									if (Pattern != "")
									{
										Pattern pat = new Pattern(Pattern, IsComplex, IsSeparator,
										                          false);
										pat.Category = Category;
										pl.Add(pat);
									}
								}
								else if (pt.Name.ToLower
									(CultureInfo.InvariantCulture) ==
									"patterns")
								{
									string Patterns = pt.ChildNodes[0].Value;
									Patterns = Patterns.Replace("\t", " ");
									while (Patterns.IndexOf("  ") >= 0)
										Patterns = Patterns.Replace("  ", " ");

									foreach (string Pattern in Patterns.Split())
									{
										if (Pattern != "")
											pl.Add(new Pattern(Pattern, false, false, false));
									}
								}
							}
						}
					}

				if (n.Name.ToLower(CultureInfo.InvariantCulture)
					== "childblocks")
				{
					foreach (XmlNode cn in n.ChildNodes)
					{
						if (cn.Name.ToLower
							(CultureInfo.InvariantCulture) == "child")
						{
							foreach (XmlAttribute att in cn.Attributes)
								if (att.Name.ToLower
									(CultureInfo.InvariantCulture) ==
									"name")
									bl.ChildBlocks.Add(GetBlock(att.Value));
						}
					}
				}
			}
		}


		//done
		private TextStyle GetStyle(string Name)
		{
			if (mStyles[Name] == null)
			{
				TextStyle s = new TextStyle();
				mStyles.Add(Name, s);
			}

			return (TextStyle) mStyles[Name];
		}

		//done
		private BlockType GetBlock(string Name)
		{
			if (mBlocks[Name] == null)
			{
				BlockType b = new BlockType
					(mLanguage);
				mBlocks.Add(Name, b);
			}

			return (BlockType) mBlocks[Name];
		}

		//done
		private void ParseStyle(XmlNode node)
		{
			string Name = "";
			string ForeColor = "", BackColor = "";
			bool Bold = false, Italic = false, Underline = false;


			foreach (XmlAttribute att in node.Attributes)
			{
				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "name")
					Name = att.Value;

				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "forecolor")
					ForeColor = att.Value;

				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "backcolor")
					BackColor = att.Value;

				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "bold")
					Bold = bool.Parse(att.Value);

				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "italic")
					Italic = bool.Parse(att.Value);

				if (att.Name.ToLower(CultureInfo.InvariantCulture)
					== "underline")
					Underline = bool.Parse(att.Value);
			}

			TextStyle st = GetStyle(Name);

			if (BackColor != "")
			{
				st.BackColor = Color.FromName(BackColor);
			}
			else
			{
			}

			st.ForeColor = Color.FromName(ForeColor);
			st.Bold = Bold;
			st.Italic = Italic;
			st.Underline = Underline;
			st.Name = Name;
		}
	}
}