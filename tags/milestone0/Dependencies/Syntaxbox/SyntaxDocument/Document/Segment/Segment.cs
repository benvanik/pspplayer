// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

namespace Puzzle.SourceCode
{
	/// <summary>
	/// 
	/// </summary>
	public class Segment
	{
		/// <summary>
		/// The owner BlockType
		/// </summary>
		public BlockType BlockType;

		/// <summary>
		/// The parent segment
		/// </summary>
		public Segment Parent;

		/// <summary>
		/// The depth of this segment in the segment hirarchy
		/// </summary>
		public int Depth = 0;

		/// <summary>
		/// The row on which the segment starts
		/// </summary>
		public Row StartRow;

		/// <summary>
		/// The word that starts this segment
		/// </summary>
		public Word StartWord;

		/// <summary>
		/// The row that the segment ends on
		/// </summary>
		public Row EndRow;

		/// <summary>
		/// The word that ends this segment
		/// </summary>
		public Word EndWord;

		/// <summary>
		/// Gets or Sets if this segment is expanded
		/// </summary>
		public bool Expanded = true;

		/// <summary>
		/// Gets or Sets what scope triggered this segment
		/// </summary>
		public Scope Scope = null;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="startrow"></param>
		public Segment(Row startrow)
		{
			StartRow = startrow;
		}

		/// <summary>
		/// 
		/// </summary>
		public Segment()
		{
		}
	}
}