using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace Simplicode.Imaging.Filters
{
	/// <summary>
	/// Crops an image, either by specifying a shrink factor, or an actual area to crop
	/// </summary>
	public class CropFilter : IImageFilter
	{
		#region Crop enum
		private enum CropMode { ShrinkFactor, Area };
		#endregion

		private CropMode _cropMode;

		/// <summary>
		/// Gets or sets the crop area.
		/// </summary>
		public Rectangle CropArea { get; private set; }

		/// <summary>
		/// Gets or sets the anchor.
		/// </summary>
		public AnchorLocation Anchor  { get; private set; }

		/// <summary>
		/// Gets or sets the shrink percant.
		/// </summary>
		public double ShrinkFactor { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CropFilter"/> class.
		/// </summary>
		/// <param name="shrinkFactor">The shrink factor.</param>
		public CropFilter(double shrinkFactor)
			: this(shrinkFactor, AnchorLocation.Middle)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CropFilter"/> class.
		/// </summary>
		/// <param name="shrinkFactor">The shrink factor.</param>
		/// <param name="anchor">The anchor.</param>
		public CropFilter(double shrinkFactor, AnchorLocation anchor)
		{
			if (0 > shrinkFactor || shrinkFactor > 1)
				throw new ArgumentException("Shrink factor must be between 0 and 1");

			this.ShrinkFactor = shrinkFactor;
			this.Anchor = anchor;
			this._cropMode = CropMode.ShrinkFactor;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CropFilter"/> class.
		/// </summary>
		/// <param name="cropArea">The crop area.</param>
		public CropFilter(Rectangle cropArea)
		{
			this.CropArea = cropArea;
			this._cropMode = CropMode.Area;
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
			Size imageSize = inputImage.Size;
			Rectangle sourceRect = Rectangle.Empty;

			if (_cropMode == CropMode.Area)
			{
				sourceRect = this.CropArea;

				int x2 = sourceRect.X + sourceRect.Width;
				if (x2 > inputImage.Width)
					sourceRect.Width -= (x2 - inputImage.Width);

				int y2 = sourceRect.Y + sourceRect.Height;
				if (y2 > inputImage.Height)
					sourceRect.Height -= (y2 - inputImage.Height);
			}
			else if (_cropMode == CropMode.ShrinkFactor)
			{
				Rectangle cropped = new Rectangle(0, 0, (int)((double)imageSize.Width * ShrinkFactor), (int)((double)imageSize.Height * ShrinkFactor));

				// Position this cropped image within the main image bounds, respecting the chosen anchor
				RectangleUtil.PositionRectangle(this.Anchor, new Rectangle(Point.Empty, imageSize), ref cropped);

				sourceRect = cropped;
			}
			
			// Write the final image
			Bitmap outputBitmap = new Bitmap(sourceRect.Width, sourceRect.Height);
			
			using (Graphics g = Graphics.FromImage(outputBitmap))
			{
				Rectangle destRect = new Rectangle(0, 0, outputBitmap.Width, outputBitmap.Height);
				g.DrawImage(inputImage, destRect, sourceRect, GraphicsUnit.Pixel);
			}

			return outputBitmap;
		}
	}
}
