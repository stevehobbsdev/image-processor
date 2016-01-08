using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplicode.Imaging.Filters.Pixel
{
	/// <summary>
	/// Converts the input image to Sepiatone
	/// </summary>
	public unsafe class SepiaPixelFilter : PixelFilter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SepiaPixelFilter"/> class.
		/// </summary>
		public SepiaPixelFilter()
		{
		}

		/// <summary>
		/// Modifies the pixel.
		/// </summary>
		/// <param name="ptr">A pointer to the current pixel</param>
		protected override void ModifyPixel(IntPtr ptr)
		{
			byte* bPtr = (byte*)ptr;
			byte r = bPtr[PIXEL_INDEX_R];
			byte g = bPtr[PIXEL_INDEX_G];
			byte b = bPtr[PIXEL_INDEX_B];

			double r2 = Math.Min((r * .393) + (g * .768) + (b * .189), 255);
			double g2 = Math.Min((r * .349) + (g * .686) + (b * .168), 255);
			double b2 = Math.Min((r * .272) + (g * .534) + (b * .131), 255);

			bPtr[PIXEL_INDEX_R] = (byte)r2;
			bPtr[PIXEL_INDEX_G] = (byte)g2;
			bPtr[PIXEL_INDEX_B] = (byte)b2;
		}
	}
}
