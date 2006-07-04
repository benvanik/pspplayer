using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Noxa.Utilities.Controls
{
	/// <summary>
	/// Serialization helper for <see cref="WhidbeyTabPage"/> instances.
	/// </summary>
	internal class WhidbeyTabPageConverter
		: TypeConverter
	{
		#region Data

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public WhidbeyTabPageConverter()
		{
		}

		#endregion

		#region Accessors

		#endregion

		#region Methods

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if( destinationType == typeof( InstanceDescriptor ) )
				return( true );

			return( base.CanConvertTo( context, destinationType ) );
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if( destinationType != typeof( InstanceDescriptor ) )
				return( base.ConvertTo( context, culture, value, destinationType ) );

			ConstructorInfo info = typeof( WhidbeyTabPage ).GetConstructor( Type.EmptyTypes );

			return( new InstanceDescriptor( info, null, false ) );
		}

		#endregion

		#region Internal methods

		#endregion
	}
}