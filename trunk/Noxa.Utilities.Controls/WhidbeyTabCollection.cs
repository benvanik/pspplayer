using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Noxa.Utilities.Controls
{
    /// <summary>
    /// Collection of <see cref="WhidbeyTabPage"/> instances.
    /// </summary>
    public class WhidbeyTabCollection
        : Collection<WhidbeyTabPage>
    {
        #region Data

        private     WhidbeyTabControl               m_oControl;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="control">Parent control to send messages to.</param>
        internal WhidbeyTabCollection(WhidbeyTabControl control)
        {
			m_oControl = control;
        }

        #endregion

        #region Accessors

        #region Internal

        /// <summary>
        /// Get the tab control which notifications should be sent to.
        /// </summary>
        private WhidbeyTabControl Control
        {
            get{ return( m_oControl ); }
        }

        #endregion

        #endregion

        #region Methods

        #endregion

        #region Internal methods

        protected override void InsertItem(int index, WhidbeyTabPage item)
        {
			this.Items.Insert( index, item );

            // Call parent to do add logic
            m_oControl.InsertTab( item );
        }

        protected override void SetItem(int index, WhidbeyTabPage item)
        {
            // FIXME: Allow sets, maybe?
            throw new NotSupportedException( "Set is not an allowed operation on this control." );
        }

        protected override void RemoveItem(int index)
        {
			// Call parent to do remove logic
            m_oControl.RemoveTab( this[ index ] );

			this.Items.RemoveAt( index );
		}

        protected override void ClearItems()
        {
			// Call parent to do clear logic
            m_oControl.ClearTabs();

			this.Items.Clear();
        }

        #endregion
    }
}