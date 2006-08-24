// ----------------------------------------------------------------------------
// Shared Utility Library
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Noxa.Utilities.Controls
{
	/// <summary>
	/// Designer for <see cref="WhidbeyTabControl"/> instances.
	/// </summary>
	internal class WhidbeyTabControlDesigner
		: ParentControlDesigner
	{
		#region Data

		private		ISelectionService				m_oSelectionSvc		= null;
		private		IComponentChangeService			m_oChangeSvc		= null;

		private		WhidbeyTabControl				m_oControl			= null;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public WhidbeyTabControlDesigner()
		{
		}

		/// <summary>
		/// Dispose.
		/// </summary>
		/// <param name="disposing"><see langword="false"/> when being called from GC.</param>
		protected override void Dispose(bool disposing)
		{
			if( m_oSelectionSvc != null )
				m_oSelectionSvc.SelectionChanged -= new EventHandler( this.OnSelectionChanged );
			if( m_oChangeSvc != null )
				m_oChangeSvc.ComponentRemoving -= new ComponentEventHandler( this.OnComponentRemoving );

			// Go down
			base.Dispose( disposing );
		}

		#endregion

		#region Accessors

		public override ICollection AssociatedComponents
		{
			get
			{
				return( m_oControl.TabPages as ICollection );
			}
		}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				DesignerVerbCollection verbs = new DesignerVerbCollection( new DesignerVerb[]{
					new DesignerVerb( "Add tab", new EventHandler( OnAddTab ) )
				} );

				return ( verbs );
			}
		}

		#endregion

		#region Methods

		public override void Initialize(IComponent component)
		{
			base.Initialize( component );

			// Store local our component
			m_oControl = component as WhidbeyTabControl;

			// Hook selection events
			m_oSelectionSvc = this.GetService( typeof( ISelectionService ) ) as ISelectionService;
			m_oSelectionSvc.SelectionChanged += new EventHandler( OnSelectionChanged );

			// Hook component add/remove/etc events
			m_oChangeSvc = this.GetService( typeof( IComponentChangeService ) ) as IComponentChangeService;
			m_oChangeSvc.ComponentRemoving += new ComponentEventHandler( OnComponentRemoving );
		}

		#endregion

		#region Internal methods

		/// <summary>
		/// Add a new tab and set selection.
		/// </summary>
		/// <param name="sender">Ignored.</param>
		/// <param name="e">Ignored.</param>
		private void OnAddTab(object sender, EventArgs e)
		{
			// Begin transaction
			IDesignerHost host = this.GetService( typeof( IDesignerHost ) ) as IDesignerHost;
			DesignerTransaction transaction = host.CreateTransaction( "Add tab" );

			// Create new tab
			WhidbeyTabPage tab = host.CreateComponent( typeof( WhidbeyTabPage ) ) as WhidbeyTabPage;
			m_oChangeSvc.OnComponentChanging( m_oControl, null );
			m_oControl.TabPages.Add( tab );
			m_oChangeSvc.OnComponentChanged( m_oControl, null, null, null );

			// End transaction
			transaction.Commit();

			m_oControl.SelectedTab = tab;
		}

		/// <summary>
		/// Fired when selection changes.
		/// </summary>
		/// <param name="sender">Ignored.</param>
		/// <param name="e">Ignored.</param>
		private void OnSelectionChanged(object sender, EventArgs e)
		{
			// Anything needed?
		}

		/// <summary>
		/// Fired when a component is removed.
		/// </summary>
		/// <param name="sender">Ignored.</param>
		/// <param name="e">Information about component.</param>
		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			IDesignerHost host = this.GetService( typeof( IDesignerHost ) ) as IDesignerHost;

			if( e.Component is WhidbeyTabControl )
			{
				while( m_oControl.TabPages.Count > 0 )
				{
					m_oChangeSvc.OnComponentChanging( m_oControl, null );
					WhidbeyTabPage tab = m_oControl.TabPages[ 0 ];

					m_oControl.TabPages.RemoveAt( 0 );
					host.DestroyComponent( tab );

					m_oChangeSvc.OnComponentChanged( m_oControl, null, null, null );
				}
			}
			else if( e.Component is WhidbeyTabPage )
			{
				WhidbeyTabPage tab = e.Component as WhidbeyTabPage;
				if( m_oControl.TabPages.Contains( tab ) == true )
				{
					m_oChangeSvc.OnComponentChanging( m_oControl, null );
					m_oControl.TabPages.Remove( tab );
					m_oChangeSvc.OnComponentChanged( m_oControl, null, null, null );
				}
			}
		}

		/// <summary>
		/// Called to check for a hit test.
		/// </summary>
		/// <param name="point">Position to check.</param>
		/// <returns><see langword="true"/> when an item is hit.</returns>
		protected override bool GetHitTest(System.Drawing.Point point)
		{
			// Put in control space
			point = m_oControl.PointToClient( point );

			foreach( WhidbeyTabPage tab in m_oControl.TabPages )
			{
				if( tab.ItemBounds.Contains( point ) == true )
					return( true );
			}

			return( false );
		}

		#endregion
	}
}