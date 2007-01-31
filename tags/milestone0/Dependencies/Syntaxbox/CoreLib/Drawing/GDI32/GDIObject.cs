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

namespace Puzzle.Drawing.GDI
{
	/// <summary>
	/// Summary description for GDIObject.
	/// </summary>
	public abstract class GDIObject : IDisposable
	{
		protected bool IsCreated = false;

		protected virtual void Destroy()
		{
			IsCreated = false;
			MemHandler.Remove(this);
		}

		protected virtual void Create()
		{
			IsCreated = true;
			MemHandler.Add(this);
		}

		#region Implementation of IDisposable

		public void Dispose()
		{
			this.Destroy();
		}

		#endregion
	}
}