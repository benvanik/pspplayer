// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System.Collections;
using System.Globalization;
using System.Text;

namespace Puzzle.SourceCode.SyntaxDocumentParsers
{
	public sealed class ParseTools
	{
		public static char[] Parse(string text, string separators)
		{
			//string Result="";
			StringBuilder Result = new StringBuilder();
			text = " " + text + " ";
			foreach (char c in text.ToCharArray())
				if (separators.IndexOf(c) >= 0)
					Result.Append(' ');
				else
					Result.Append('.');
			return Result.ToString().ToCharArray();
		}


		public static void AddPatternString(string Text, Row Row, Pattern Pattern,
		                                    TextStyle Style, Segment Segment, bool
		                                    	HasError)
		{
			Word x = Row.Add(Text);
			x.Style = Style;
			x.Pattern = Pattern;
			x.HasError = HasError;
			x.Segment = Segment;
		}

		public static unsafe void AddString(string Text, Row Row, TextStyle Style,
		                                    Segment Segment)
		{
			if (Text == "")
				return;

			StringBuilder CurrentWord = new StringBuilder();
			char[] Buff = Text.ToCharArray();
			fixed (char* c = &Buff[0])
			{
				for (int i = 0; i < Text.Length; i++)
				{
					if (c[i] == ' ' || c[i] == '\t')
					{
						if (CurrentWord.Length != 0)
						{
							Word word = Row.Add(CurrentWord.ToString());
							word.Style = Style;
							word.Segment = Segment;
							CurrentWord = new StringBuilder();
						}

						Word ws = Row.Add(c[i].ToString
							(CultureInfo.InvariantCulture));
						if (c[i] == ' ')
							ws.Type = WordType.xtSpace;
						else
							ws.Type = WordType.xtTab;
						ws.Style = Style;
						ws.Segment = Segment;
					}
					else
						CurrentWord.Append(c[i].ToString
							(CultureInfo.InvariantCulture));
				}
				if (CurrentWord.Length != 0)
				{
					Word word = Row.Add(CurrentWord.ToString());
					word.Style = Style;
					word.Segment = Segment;
				}
			}
		}


		public static ArrayList GetWords(string text)
		{
			ArrayList words = new ArrayList();
			StringBuilder CurrentWord = new StringBuilder();
			foreach (char c in text.ToCharArray())
			{
				if (c == ' ' || c == '\t')
				{
					if (CurrentWord.ToString() != "")
					{
						words.Add(CurrentWord.ToString());
						CurrentWord = new StringBuilder();
					}

					words.Add(c.ToString
						(CultureInfo.InvariantCulture));

				}
				else
					CurrentWord.Append(c.ToString
						(CultureInfo.InvariantCulture)
						);
			}
			if (CurrentWord.ToString() != "")
				words.Add(CurrentWord.ToString());
			return words;
		}

		public static PatternScanResult GetFirstWord(char[] TextBuffer,
		                                             PatternCollection Patterns, int StartPosition)
		{
			PatternScanResult Result;
			Result.Index = 0;
			Result.Token = "";

			//			for (int i=StartPosition;i<TextBuffer.Length;i++)
			//			{
			//
			//				//-----------------------------------------------
			//				if (c[i]==PatternBuffer[0])
			//				{
			//					bool found=true;
			//					for (int j=0;j<Pattern.Length;j++)
			//					{
			//						if (c[i+j]!=p[j])
			//						{
			//							found=false;
			//							break;
			//						}
			//					}
			//					if (found)
			//					{
			//						Result.Index =i+StartPosition;
			//						Result.Token = Text.Substring(i+StartPosition,this.Pattern.Length);
			//						return Result;
			//					}							
			//				}
			//				//-----------------------------------------------
			//			}


			return Result;
		}
	}
}