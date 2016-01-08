using System;
using System.Drawing.Imaging;
using System.IO;

namespace Simplicode.Imaging
{
	/// <summary>
	/// Utility object
	/// </summary>
	public class Util
	{
		/// <summary>
		/// Gets the image format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>An ImageFormat instance which matches the specified OutputFormat</returns>
		public static ImageFormat GetImageFormat(ImageFormatType format)
		{
			switch (format)
			{
				case ImageFormatType.Bmp:
					return ImageFormat.Bmp;

				case ImageFormatType.Jpeg:
					return ImageFormat.Jpeg;

				case ImageFormatType.Png:
					return ImageFormat.Png;

				case ImageFormatType.Tiff:
					return ImageFormat.Tiff;

				case ImageFormatType.Gif:
					return ImageFormat.Gif;

				case ImageFormatType.Default:
					return ImageFormat.Jpeg;

				default:
					return ImageFormat.Jpeg;
			}
		}

		/// <summary>
		/// Makes a guess at the output format given the filename.
		/// </summary>
		/// <param name="filename">The filename to get the output format for.</param>
		public static ImageFormatType DiscoverOutputFormatFromFile(string filename)
		{
			string extension = Path.GetExtension(filename);

			switch (extension.ToLower())
			{
				case ".jpg":
				case ".jpeg":
					return ImageFormatType.Jpeg;
					
				case ".png":
					return ImageFormatType.Png;

				case ".bmp":
					return ImageFormatType.Bmp;

				case ".gif":
					return ImageFormatType.Gif;

				case ".tiff":
					return ImageFormatType.Tiff;

				default:
					return ImageFormatType.Default;
			}
		}

		/// <summary>
		/// Finds the appropriate ImageCodecInfo instance based on the image format.
		/// </summary>
		/// <param name="fmt">The image format</param>
		/// <returns>The image codec which can be used to save an image in the input image format.</returns>
		public static ImageCodecInfo FindCodecInfo(ImageFormat fmt)
		{
			ImageCodecInfo[] infoArray1 = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo[] infoArray2 = infoArray1;
			for (int num1 = 0; num1 < infoArray2.Length; num1++)
			{
				ImageCodecInfo info1 = infoArray2[num1];
				if (info1.FormatID.Equals(fmt.Guid))
				{
					return info1;
				}
			}
			return null;
		}
	}
}
