// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

namespace Puzzle.SourceCode.SyntaxDocumentParsers
{
	/// <summary>
	/// Parser interface.
	/// Implement this interface if you want to create your own parser.
	/// </summary>
	public interface IParser
	{
		/// <summary>
		/// Gets or Sets the Document object for this parser
		/// </summary>
		SyntaxDocument Document { get; set; }

		/// <summary>
		/// Gets or Sets the Language for this parser
		/// </summary>
		Language Language { get; set; }

		string Separators { get; set; }

		/// <summary>
		/// Initializes the parser with a spcified SyntaxFile
		/// </summary>
		/// <param name="SyntaxFile">Filename of the SyntaxFile that should be used</param>
		void Init(string SyntaxFile);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="SyntaxFile"></param>
		/// <param name="Separators"></param>
		void Init(string SyntaxFile, string Separators);

		/// <summary>
		/// Initializes the parser with a spcified Language object
		/// </summary>
		/// <param name="Language">The Language object to assign to the parser</param>
		void Init(Language Language);

		/// <summary>
		/// Called by the SyntaxDocument object when a row should be parsed
		/// </summary>
		/// <param name="RowIndex">The row index in the document</param>
		/// <param name="ParseKeywords">true if keywords and operators should be parsed , false if only a segment parse should be performed</param>
		void ParseLine(int RowIndex, bool ParseKeywords);

		/// <summary>
		/// Called by the SyntaxDocument object when a row must be preview parsed.
		/// </summary>
		/// <param name="RowIndex">Row index in the document</param>
		void ParsePreviewLine(int RowIndex);
	}
}