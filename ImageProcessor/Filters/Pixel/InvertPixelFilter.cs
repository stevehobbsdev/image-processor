using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplicode.Imaging.Filters.Pixel
{
	/// <summary>
	/// Inverts the pixels of the input image.
	/// </summary>
	public unsafe class InvertPixelFilter : PixelFilter
	{
		/// <summary>
		/// Modifies the pixel.
		/// </summary>
		/// <param name="ptr">A pointer to the current pixel</param>
		protected override void ModifyPixel(IntPtr ptr)
		{
			byte* bPtr = (byte*)ptr;

			bPtr[PIXEL_INDEX_R] = (byte)Math.Max((255 - bPtr[PIXEL_INDEX_R]), 0);
			bPtr[PIXEL_INDEX_G] = (byte)Math.Max((255 - bPtr[PIXEL_INDEX_G]), 0);
			bPtr[PIXEL_INDEX_B] = (byte)Math.Max((255 - bPtr[PIXEL_INDEX_B]), 0);
		}
	}
}
