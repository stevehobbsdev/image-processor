using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Simplicode.Imaging.ProviderModel
{
	public interface IImageProvider
	{
		/// <summary>
		/// Reads the images from the specified image stream
		/// </summary>
		/// <param name="imageStream">The image stream.</param>
		/// <returns>An array of images that were found in the stream</returns>
		IEnumerable<Image> Read(Stream imageStream);

		/// <summary>
		/// Writes the specified images to the output stream
		/// </summary>
		/// <param name="images">The images.</param>
		/// <param name="outputStream">The image stream.</param>
		/// <param name="outputFormat">The output format.</param>
		/// <param name="quality">The quality.</param>
		void Write(IEnumerable<Image> images, Stream imageStream, ImageFormatType outputFormat, long quality);
	}
}
