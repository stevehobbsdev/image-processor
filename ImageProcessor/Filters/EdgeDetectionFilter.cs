using System;
using System.Drawing;

namespace Simplicode.Imaging.Filters
{
	/// <summary>
	/// Applies a simple edge detection algorithm to an image
	/// </summary>
	public class EdgeDetectionFilter : IImageFilter
	{
		/// <summary>
		/// Gets or sets the edge threshold.
		/// </summary>
		public double EdgeThreshold { get; set; }

		/// <summary>
		/// Gets or sets the base colour.
		/// </summary>
		public Color BaseColour { get; set; }

		/// <summary>
		/// Gets or sets the edge colour.
		/// </summary>
		public Color EdgeColour { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="EdgeDetectionFilter"/> class.
		/// </summary>
		public EdgeDetectionFilter()
			: this(200)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EdgeDetectionFilter"/> class.
		/// </summary>
		/// <param name="edgeThreshold">The edge threshold.</param>
		public EdgeDetectionFilter(double edgeThreshold)
			: this(edgeThreshold, Color.Black, Color.White)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EdgeDetectionFilter"/> class.
		/// </summary>
		/// <param name="edgeThreshold">The edge threshold.</param>
		/// <param name="baseColour">The base colour.</param>
		/// <param name="edgeColour">The edge colour.</param>
		public EdgeDetectionFilter(double edgeThreshold, Color baseColour, Color edgeColour)
		{
			this.EdgeThreshold = edgeThreshold;
			this.BaseColour = baseColour;
			this.EdgeColour = edgeColour;
		}

		/// <summary>
		/// Performs the processing of an image.
		/// </summary>
		/// <param name="inputImage">The image to perform the processing on.</param>
		/// <returns>
		/// The image after it has been processed by this filter.
		/// </returns>
		public virtual Image Process(System.Drawing.Image inputImage)
		{
			Bitmap bmp = new Bitmap(inputImage);

			for (int i = 1; i < (bmp.Width - 1); i++)
			{
				for (int j = 1; j < (bmp.Height - 1); j++)
				{
					Color color = bmp.GetPixel(i, j);
					double r = color.R;
					double g = color.G;
					double b = color.B;

					Color color1 = bmp.GetPixel(i + 1, j);
					double r1 = color1.R;
					double g1 = color1.G;
					double b1 = color1.B;

					Color color2 = bmp.GetPixel(i, j + 1);
					double r2 = color2.R;
					double g2 = color2.G;
					double b2 = color2.B;

					double c1 = Math.Sqrt((r - r1) * (r - r1)) + ((g - g1) + (g - g1)) + ((b - b1) * (b - b1));
					double c2 = Math.Sqrt(((r - r2) * (r - r2)) + ((g - g2) + (g - g2)) + ((b - b2) * (b - b2)));

					if ((c1 > EdgeThreshold) || (c2 > EdgeThreshold))
					{
						bmp.SetPixel(i, j, EdgeColour);
					}
					else
					{
						bmp.SetPixel(i, j, BaseColour);
					}

				}
			}

			return bmp;
		}
	}
}
