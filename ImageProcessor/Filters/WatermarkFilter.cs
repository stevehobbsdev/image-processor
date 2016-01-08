using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Simplicode.Imaging.Filters
{
	/// <summary>
	/// Base class for filters which apply a watermark
	/// </summary>
	public abstract class WatermarkFilter : IImageFilter
	{
		/// <summary>
		/// Gets or sets the anchor location.
		/// </summary>
		public AnchorLocation AnchorLocation { get; set; }

		/// <summary>
		/// Gets or sets the offset for the watermark
		/// </summary>
		public Point Offset { get; set; }

		/// <summary>
		/// Gets or sets the opacity,
		/// </summary>
		public float Opacity
		{
			get { return _opacity; }
			set
			{
				if (value < 0)
					_opacity = 0;
				else if (value > 1)
					_opacity = 1;
				else
					_opacity = value;
			}
		}
		private float _opacity;

		/// <summary>
		/// Performs the processing of an image.
		/// </summary>
		/// <param name="inputImage">The image to perform the processing on.</param>
		/// <returns>
		/// The image after it has been processed by this filter.
		/// </returns>
		public abstract Image Process(Image inputImage);

	}
}
