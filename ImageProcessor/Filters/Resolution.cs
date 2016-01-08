using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplicode.Imaging.Filters
{
	/// <summary>
	/// Represents a resolution given the X and Y axes.
	/// </summary>
	public struct Resolution
	{
		/// <summary>
		/// Gets or sets the X resolution
		/// </summary>
		public float X { get; set; }

		/// <summary>
		/// Gets or sets the Y resolution
		/// </summary>
		public float Y { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Resolution"/> struct.
		/// </summary>
		/// <param name="x">The x resolution</param>
		/// <param name="y">The y resolution</param>
		public Resolution(float x, float y) : this()
		{
			this.X = x;
			this.Y = y;
		}

		#region Common sizes

		/// <summary>
		/// Returns an instance of <see cref="Resolution"/> for screen imaging
		/// </summary>
		public static Resolution Screen
		{
			get
			{
				return new Resolution(72, 72);
			}
		}

		/// <summary>
		/// Returns an instance of <see cref="Resolution"/> for screen imaging
		/// </summary>
		public static Resolution Print
		{
			get
			{
				return new Resolution(300, 300);
			}
		}

		// More?

		#endregion
	}
}
