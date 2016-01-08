using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Simplicode.Imaging.ProviderModel
{
	[ImageProvider(ImageFormatType.Default)]
	public class DefaultImageProvider : IImageProvider
	{
		/// <summary>
		/// Reads the images from the specified image stream
		/// </summary>
		/// <param name="imageStream">The image stream.</param>
		/// <returns>An array of images that were found in the stream</returns>
		public IEnumerable<Image> Read(Stream imageStream)
		{
			return new Image[] { new Bitmap(imageStream) };
		}

		/// <summary>
		/// Writes the specified images to the output stream
		/// </summary>
		/// <param name="images">The images.</param>
		/// <param name="outputStream">The image stream.</param>
		/// <param name="outputFormat">The output format.</param>
		/// <param name="quality">The quality.</param>
		public void Write(IEnumerable<Image> images, Stream outputStream, ImageFormatType outputFormat, long quality)
		{
			if (images.Count() > 0)
			{
				EncoderParameters codecParams = new EncoderParameters(1);

				codecParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

				ImageCodecInfo codecInfo = Util.FindCodecInfo(Util.GetImageFormat(outputFormat));

				images.First().Save(outputStream, codecInfo, codecParams);
			}
		}
	}
}
