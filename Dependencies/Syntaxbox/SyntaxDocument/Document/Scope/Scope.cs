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
	/// The Scope class defines what patterns starts and ends a BlockType
	/// </summary>
	public sealed class Scope
	{
		/// <summary>
		/// The Start trigger Pattern
		/// </summary>
		public Pattern Start = null;

		/// <summary>
		/// BlockType that should be started directly after this block have started
		/// </summary>
		public BlockType SpawnBlockOnStart = null;

		/// <summary>
		/// BlockType that should be started directly after this block have ended
		/// </summary>
		public BlockType SpawnBlockOnEnd = null;

		/// <summary>
		/// The owner BlockType
		/// </summary>
		public BlockType Parent = null;

		/// <summary>
		/// List of end patterns
		/// </summary>
		public PatternCollection EndPatterns = new PatternCollection();

		/// <summary>
		/// The style that should be applied to the start and end patterns
		/// </summary>
		public TextStyle Style = null;

		/// <summary>
		/// The text that should be displayed if the owner block is collapsed
		/// </summary>
		public string ExpansionText = "";

		/// <summary>
		/// Gets or Sets if this block should be expanded or collapsed by default
		/// </summary>
		public bool DefaultExpanded = true;

		/// <summary>
		/// Gets or Sets if the scope patterns is case sensitive
		/// </summary>
		public bool CaseSensitive = false;

		/// <summary>
		/// Gets or Sets if the scope patterns should be case normalized
		/// </summary>
		public bool NormalizeCase = true;

		public bool CauseIndent = false;

		/// <summary>
		/// 
		/// </summary>
		public Scope()
		{
		}
	}
}