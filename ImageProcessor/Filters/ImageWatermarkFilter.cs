using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Simplicode.Imaging.Filters
{
	/// <summary>
	/// Watermarks the processed image with another smaller image.
	/// </summary>
	public class ImageWatermarkFilter : WatermarkFilter
	{
		/// <summary>
		/// Gets the image being used as the watermark
		/// </summary>
		public Image WatermarkImage { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageWatermarkFilter"/> class.
		/// </summary>
		/// <param name="watermarkImagePath">The watermark image path.</param>
		/// <param name="anchorLocation">The anchor location.</param>
		public ImageWatermarkFilter(string watermarkImagePath, AnchorLocation anchorLocation = AnchorLocation.BottomRight)
			: this(Image.FromFile(watermarkImagePath), anchorLocation)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageWatermarkFilter"/> class.
		/// </summary>
		/// <param name="watermarkImage">The watermark image.</param>
		/// <param name="anchorLocation">The anchor location.</param>
		public ImageWatermarkFilter(Image watermarkImage, AnchorLocation anchorLocation = AnchorLocation.BottomRight)
		{
			this.WatermarkImage = watermarkImage;
			this.AnchorLocation = anchorLocation;
		}

		/// <summary>
		/// Performs the processing of an image.
		/// </summary>
		/// <param name="inputImage">The image to perform the processing on.</param>
		/// <returns>
		/// The image after it has been processed by this filter.
		/// </returns>
		public override Image Process(Image inputImage)
		{
			using (Graphics g = Graphics.FromImage(inputImage))
			{

				g.CompositingQuality = CompositingQuality.HighQuality;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;

				// Figure out the position based on the anchor location
				Rectangle imageRect = new Rectangle(Point.Empty, inputImage.Size);
				Rectangle watermarkRect = new Rectangle(Point.Empty, WatermarkImage.Size);

				RectangleUtil.PositionRectangle(this.AnchorLocation, imageRect, ref watermarkRect);

				watermarkRect.Offset(this.Offset);

				ImageAttributes imageAttr = BuildImageAttributes();

				g.DrawImage(this.WatermarkImage, watermarkRect, 0, 0, WatermarkImage.Width, WatermarkImage.Height, GraphicsUnit.Pixel, imageAttr);

			}
			return inputImage;
		}

		/// <summary>
		/// Builds the image attributes.
		/// </summary>
		private ImageAttributes BuildImageAttributes()
		{
			ColorMatrix matrix = new ColorMatrix();
			matrix.Matrix33 = this.Opacity;

			ImageAttributes imageAttr = new ImageAttributes();
			imageAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

			return imageAttr;
		}
	}
}
