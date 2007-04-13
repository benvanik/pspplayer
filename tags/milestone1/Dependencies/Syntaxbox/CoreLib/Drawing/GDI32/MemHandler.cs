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

namespace Puzzle.Drawing.GDI
{
	/// <summary>
	/// Summary description for MemHandler.
	/// </summary>
	public class MemHandler
	{
		private static ArrayList mHeap = new ArrayList();

		public static void Add(GDIObject item)
		{
			mHeap.Add(item);
		}

		public static void Remove(GDIObject item)
		{
			if (mHeap.Contains(item))
			{
				mHeap.Remove(item);
			}
		}

		public static GDIObject[] Items
		{
			get
			{
				ArrayList al = new ArrayList();

				foreach (GDIObject go in mHeap)
				{
					if (go != null)
						al.Add(go);
				}

				GDIObject[] gos = new GDIObject[al.Count];
				al.CopyTo(0, gos, 0, al.Count);
				return gos;
			}
		}

		public static void DestroyAll()
		{
			foreach (GDIObject go in Items)
			{
				go.Dispose();
			}
		}
	}
}