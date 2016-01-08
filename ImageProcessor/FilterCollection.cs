using System.Collections;
using Simplicode.Imaging.Filters;
using System.Collections.Generic;

namespace Simplicode.Imaging
{
	/// <summary>
	/// A collection of filter objects.
	/// </summary>
	public class FilterCollection : List<IImageFilter>
	{
		#region Constructors

		/// <summary>
		/// Initialises a new instance of the filter collection.
		/// </summary>
		public FilterCollection()
		{
		}

		#endregion
	}
}
