using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Simplicode.Imaging.ProviderModel
{
	[ImageProvider(ImageFormatType.Gif)]
	public class GifImageProvider : IImageProvider
	{
		public const int DEFAULT_GIF_COLOURS = 256;

		/// <summary>
		/// Gets or sets the colour depth.
		/// </summary>
		public int ColourDepth { get; set; }

		#region IImageProvider Members

		/// <summary>
		/// Initializes a new instance of the <see cref="GifImageProvider"/> class.
		/// </summary>
		public GifImageProvider()
			: this(DEFAULT_GIF_COLOURS)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GifImageProvider"/> class.
		/// </summary>
		/// <param name="colourDepth">The colour depth.</param>
		public GifImageProvider(int colourDepth)
		{
			this.ColourDepth = colourDepth;
		}

		/// <summary>
		/// Reads the images from the specified image stream
		/// </summary>
		/// <param name="imageStream">The image stream.</param>
		/// <returns>
		/// An array of images that were found in the stream
		/// </returns>
		public IEnumerable<System.Drawing.Image> Read(System.IO.Stream imageStream)
		{
			return new Image[] { new Bitmap(imageStream) };
		}

		/// <summary>
		/// Writes the specified images to the output stream
		/// </summary>
		/// <param name="images">The images.</param>
		/// <param name="imageStream"></param>
		/// <param name="outputFormat">The output format.</param>
		/// <param name="quality">The quality.</param>
		public void Write(IEnumerable<System.Drawing.Image> images, System.IO.Stream imageStream, ImageFormatType outputFormat, long quality)
		{
			if (images.Count() > 0)
			{
				OctreeQuantizer quantizer = new OctreeQuantizer(this.ColourDepth - 1, 8);

				Bitmap quantized = quantizer.Quantize(images.First());
				quantized.Save(imageStream, ImageFormat.Gif);
			}
		}

		#endregion
	}
}
