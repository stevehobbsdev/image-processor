using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Diagnostics;

namespace Simplicode.Imaging.Filters.Pixel
{
	/// <summary>
	/// A type of filter that processes single pixels in isolation.
	/// </summary>
	public unsafe class PixelFilter : IImageFilter
	{
#pragma warning disable 1591
		//protected const byte PIXEL_INDEX_A = 3;
		protected const byte PIXEL_INDEX_R = 2;
		protected const byte PIXEL_INDEX_G = 1;
		protected const byte PIXEL_INDEX_B = 0;
#pragma warning restore 1591

		private Action<IntPtr> _pixelFunc;

		/// <summary>
		/// Initializes a new instance of the <see cref="PixelFilter"/> class.
		/// </summary>
		protected PixelFilter()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PixelFilter"/> class.
		/// </summary>
		/// <param name="pixelFunc">The pixel func.</param>
		public PixelFilter(Action<IntPtr> pixelFunc)
		{
			_pixelFunc = pixelFunc;
		}

		/// <summary>
		/// Performs the processing of an image.
		/// </summary>
		/// <param name="inputImage">The image to perform the processing on.</param>
		/// <returns>
		/// The image after it has been processed by this filter.
		/// </returns>
		public Image Process(Image inputImage)
		{
			Bitmap bmp = new Bitmap(inputImage);
			BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			// Modify the data
			byte* ptr = (byte*)data.Scan0.ToPointer();

			int padding = data.Stride - (bmp.Width * 3);

			for (int y = 0; y < bmp.Height; y++)
			{
				for (int x = 0; x < bmp.Width; x++)
				{
					ModifyPixel((IntPtr)ptr);

					ptr += 3;
				}

				ptr += padding;
			}

			bmp.UnlockBits(data);

			return bmp;
		}

		/// <summary>
		/// Modifies the pixel.
		/// </summary>
		/// <param name="ptr">A pointer to the current pixel</param>
		protected virtual void ModifyPixel(IntPtr ptr)
		{
			_pixelFunc(ptr);
		}
	}
}
