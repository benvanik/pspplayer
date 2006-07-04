using System;
using System.Collections.Generic;
using System.Text;

namespace Noxa
{
	/// <summary>
	/// Tools to help with operations dealing with randomness.
	/// </summary>
	public static class RandomTools
	{
		/// <summary>
		/// Pair of probability and it's matching item.
		/// </summary>
		/// <typeparam name="T">Item type.</typeparam>
		public struct ProbabilityPair<T>
		{
			public float Probability;
			public T Item;

			public ProbabilityPair( float probability, T item )
			{
				this.Probability = probability;
				this.Item = item;
			}
		}

		/// <summary>
		/// Pick an item from the list with the given probabilities.
		/// </summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="random">Random source.</param>
		/// <param name="probabilities">List of probability/item pairs to pick from.</param>
		/// <returns>The picked item or <c>null</c> if an error occured.</returns>
		public static T ProbabilisticPick<T>( FastRandom random, List<ProbabilityPair<T>> probabilities )
		{
			float sum = 0.0f;
			for( int n = 0; n < probabilities.Count; n++ )
			{
				ProbabilityPair<T> pair = probabilities[ n ];
				sum += pair.Probability;
			}

			float value = random.NextSingle() * sum;
			float current = 0.0f;
			for( int n = 0; n < probabilities.Count; n++ )
			{
				ProbabilityPair<T> pair = probabilities[ n ];
				current += pair.Probability;
				if( value <= current )
					return pair.Item;
			}

			return default( T );
		}
	}
}
