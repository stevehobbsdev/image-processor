using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplicode.Imaging.Filters.Pixel
{
	/// <summary>
	/// Converts the input image to grayscale
	/// </summary>
	public unsafe class GrayscalePixelFilter : PixelFilter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GrayscalePixelFilter"/> class.
		/// </summary>
		public GrayscalePixelFilter()
		{
		}

		/// <summary>
		/// Modifies the pixel.
		/// </summary>
		/// <param name="ptr">A pointer to the current pixel</param>
		protected override void ModifyPixel(IntPtr ptr)
		{
			byte* bptr = (byte*)ptr;
			byte l = (byte)((bptr[PIXEL_INDEX_R] * .3) + (bptr[PIXEL_INDEX_G] * .59) + (bptr[PIXEL_INDEX_B] * .11));

			l = Math.Min(l, (byte)255);

			bptr[PIXEL_INDEX_R] = l;
			bptr[PIXEL_INDEX_G] = l;
			bptr[PIXEL_INDEX_B] = l;
		}
	}
}
