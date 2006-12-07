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

namespace Puzzle.SourceCode
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class PatternListList : IEnumerable
	{
		private ArrayList mGroups = new ArrayList();

		/// <summary>
		/// 
		/// </summary>
		public BlockType Parent = null;

		/// <summary>
		/// 
		/// </summary>
		public bool IsKeyword = false;

		/// <summary>
		/// 
		/// </summary>
		public bool IsOperator = false;

		/// <summary>
		/// 
		/// </summary>
		public PatternListList()
		{
		}

		public PatternListList(BlockType parent)
		{
			this.Parent = parent;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return mGroups.GetEnumerator();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Group"></param>
		/// <returns></returns>
		public PatternList Add(PatternList Group)
		{
			mGroups.Add(Group);
			Group.Parent = this;
			if (this.Parent != null && this.Parent.Parent != null)
				this.Parent.Parent.ChangeVersion();

			return Group;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			mGroups.Clear();
		}

		/// <summary>
		/// 
		/// </summary>
		public PatternList this[int index]
		{
			get { return (PatternList) mGroups[index]; }

			set { mGroups[index] = value; }
		}


	}
}