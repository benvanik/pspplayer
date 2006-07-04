using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Utilities.Controls
{
	/// <summary>
	/// Event arguments for <see cref="WhidbeyTabControl"/> events.
	/// </summary>
	public class WhidbeyTabEventArgs
		: EventArgs
	{
		#region Data

		private		WhidbeyTabPage					m_oTab;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="tab">Tab this event is about.</param>
		internal WhidbeyTabEventArgs(WhidbeyTabPage tab)
		{
			m_oTab = tab;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Get the tab page this event is about.
		/// </summary>
		public WhidbeyTabPage Tab
		{
			get{ return( m_oTab ); }
		}

		#endregion

		#region Methods

		#endregion

		#region Internal methods

		#endregion
	}
}