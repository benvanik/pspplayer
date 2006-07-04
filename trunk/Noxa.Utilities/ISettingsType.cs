using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa
{
	public interface ISettingsType
	{
		// Also need a (string, int, bool) constructor where string is the value from Serialize(), int is a depth in the save tree

		string Serialize( int depth );
	}
}
