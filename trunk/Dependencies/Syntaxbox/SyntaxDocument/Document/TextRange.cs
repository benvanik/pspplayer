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

namespace Puzzle.SourceCode
{
	/// <summary>
	/// A range of text
	/// </summary>
	public class TextRange
	{
		public event EventHandler Change = null;

		protected virtual void OnChange()
		{
			if (Change != null)
				Change(this, EventArgs.Empty);
		}

		/// <summary>
		/// The start row of the range
		/// </summary>

		#region PUBLIC PROPERTY FIRSTROW
		private int _FirstRow = 0;

		public int FirstRow
		{
			get { return _FirstRow; }
			set
			{
				_FirstRow = value;
				OnChange();
			}
		}

		#endregion		

		/// <summary>
		/// The start column of the range
		/// </summary>

		#region PUBLIC PROPERTY FIRSTCOLUMN
		private int _FirstColumn;

		public int FirstColumn
		{
			get { return _FirstColumn; }
			set
			{
				_FirstColumn = value;
				OnChange();
			}
		}

		#endregion

		/// <summary>
		/// The end row of the range
		/// </summary>

		#region PUBLIC PROPERTY LASTROW
		private int _LastRow = 0;

		public int LastRow
		{
			get { return _LastRow; }
			set
			{
				_LastRow = value;
				OnChange();
			}
		}

		#endregion

		/// <summary>
		/// The end column of the range
		/// </summary>

		#region PUBLIC PROPERTY LASTCOLUMN
		private int _LastColumn = 0;

		public int LastColumn
		{
			get { return _LastColumn; }
			set
			{
				_LastColumn = value;
				OnChange();
			}
		}

		#endregion

		public void SetBounds(int FirstColumn, int FirstRow, int LastColumn, int LastRow)
		{
			_FirstColumn = FirstColumn;
			_FirstRow = FirstRow;
			_LastColumn = LastColumn;
			_LastRow = LastRow;
			OnChange();
		}

		public TextRange()
		{
		}

		public TextRange(int FirstColumn, int FirstRow, int LastColumn, int LastRow)
		{
			_FirstColumn = FirstColumn;
			_FirstRow = FirstRow;
			_LastColumn = LastColumn;
			_LastRow = LastRow;
		}

	}
}