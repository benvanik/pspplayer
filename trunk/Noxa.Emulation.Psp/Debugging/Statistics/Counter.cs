// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa.Emulation.Psp.Debugging.Statistics
{
	/// <summary>
	/// Defines <see cref="Counter"/> sampling modes.
	/// </summary>
	public enum CounterMode
	{
		/// <summary>
		/// Values are computed from a short history of sampled values.
		/// </summary>
		Historical,
		/// <summary>
		/// Values are computed from a rolling arithmetic mean of sampled values.
		/// </summary>
		ArithmeticMean,
	}

	/// <summary>
	/// Defines default <see cref="Counter"/> display modes.
	/// </summary>
	public enum CounterDisplayType
	{
		/// <summary>
		/// Display the last sampled value.
		/// </summary>
		LastValue,
		/// <summary>
		/// Display the rate at which the value is changing.
		/// </summary>
		Rate,
		/// <summary>
		/// Display the average of the values.
		/// </summary>
		Average,
	}

	/// <summary>
	/// Generic performance counter.
	/// </summary>
	public class Counter : MarshalByRefObject
	{
		private bool _valid;
		private double _rate;
		private double _average;

		/// <summary>
		/// The sampling mode of the counter.
		/// </summary>
		public readonly CounterMode Mode;

		/// <summary>
		/// The default display type of the counter.
		/// </summary>
		public readonly CounterDisplayType DisplayType;

		/// <summary>
		/// The name of the counter instance.
		/// </summary>
		public readonly string Name;
		/// <summary>
		/// The description of the counter instance.
		/// </summary>
		public readonly string Description;

		/// <summary>
		/// The number of samples taken of this counter.
		/// </summary>
		public uint SampleCount;

		/// <summary>
		/// The estimated rate at which this counter is increasing/decreasing.
		/// </summary>
		public double Rate
		{
			get
			{
				if( _valid == false )
					this.UpdateDerived();
				return _rate;
			}
		}

		/// <summary>
		/// The estimated average value of the counter.
		/// </summary>
		public double Average
		{
			get
			{
				if( _valid == false )
					this.UpdateDerived();
				return _average;
			}
		}

		/// <summary>
		/// The last value sampled.
		/// </summary>
		public double LastValue
		{
			get
			{
				lock( this.History )
					return this.History.PeekTail();
			}
		}

		/// <summary>
		/// The minimum reported value.
		/// </summary>
		public double MinimumValue;

		/// <summary>
		/// The maximum reported value.
		/// </summary>
		public double MaximumValue;

		/// <summary>
		/// The change between the last two sampled values.
		/// </summary>
		public double Delta;

		/// <summary>
		/// A trailing history of values.
		/// </summary>
		public CircularList<double> History;

		/// <summary>
		/// The maximum number of items to keep in the counter history.
		/// </summary>
		public const int CounterHistorySize = 5;

		/// <summary>
		/// Initialize a new <see cref="Counter"/> instance with the given parameters.
		/// </summary>
		/// <param name="name">The name of the counter.</param>
		/// <param name="description">The description of the counter.</param>
		public Counter( string name, string description )
			: this( name, description, CounterMode.Historical, CounterDisplayType.LastValue )
		{
		}

		/// <summary>
		/// Initialize a new <see cref="Counter"/> instance with the given parameters.
		/// </summary>
		/// <param name="name">The name of the counter.</param>
		/// <param name="description">The description of the counter.</param>
		/// <param name="displayType">The default display mode for the ocunter.</param>
		public Counter( string name, string description, CounterDisplayType displayType )
			: this( name, description, CounterMode.Historical, displayType )
		{
		}

		/// <summary>
		/// Initialize a new <see cref="Counter"/> instance with the given parameters.
		/// </summary>
		/// <param name="name">The name of the counter.</param>
		/// <param name="description">The description of the counter.</param>
		/// <param name="mode">The mode of the counter sampling.</param>
		public Counter( string name, string description, CounterMode mode )
			: this( name, description, mode, CounterDisplayType.LastValue )
		{
		}

		/// <summary>
		/// Initialize a new <see cref="Counter"/> instance with the given parameters.
		/// </summary>
		/// <param name="name">The name of the counter.</param>
		/// <param name="description">The description of the counter.</param>
		/// <param name="mode">The mode of the counter sampling.</param>
		/// <param name="displayType">The default display mode for the counter.</param>
		public Counter( string name, string description, CounterMode mode, CounterDisplayType displayType )
		{
			this.Mode = mode;
			this.Name = name;
			this.Description = description;

			// HACK
			this.Mode = CounterMode.Historical;

			if( this.Mode == CounterMode.Historical )
				this.History = new CircularList<double>( CounterHistorySize, true );
			this.DisplayType = displayType;
		}

		/// <summary>
		/// Update the counter with the given value.
		/// </summary>
		/// <param name="value">The newest sampled value.</param>
		public void Update( double value )
		{
			if( value < this.MinimumValue )
				this.MinimumValue = value;
			if( value > this.MaximumValue )
				this.MaximumValue = value;
			switch( this.Mode )
			{
				case CounterMode.Historical:
					lock( this.History )
					{
						this.Delta = value - this.History.PeekTail();
						this.History.Enqueue( value );
					}
					break;
				case CounterMode.ArithmeticMean:
					break;
			}
			_valid = false;
			this.SampleCount++;
		}

		private void UpdateDerived()
		{
			if( this.Mode == CounterMode.ArithmeticMean )
				return;
			lock( this.History )
			{
				double total = 0.0;
				double sum = 0.0;
				double last = 0.0;
				bool first = true;
				double count = ( double )this.History.Count;
				foreach( double sample in this.History )
				{
					sum += sample;
					if( first == true )
					{
					    last = sample;
					    first = false;
					    continue;
					}
					else
					{
					    total += ( sample - last );
					}
				}
				_average = sum / count;
				_rate = total / count;
			}
			_valid = true;
		}
	}
}
