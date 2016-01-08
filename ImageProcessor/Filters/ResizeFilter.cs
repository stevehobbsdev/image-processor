using System.Drawing;
using System.Drawing.Drawing2D;

namespace Simplicode.Imaging.Filters
{
	/// <summary>
	/// Standard filter which resizes images to a target resolution.
	/// </summary>
	public class ResizeFilter : IImageFilter
	{
		#region Properties

		/// <summary>
		/// Gets or sets the target image resolution
		/// </summary>
		public Size TargetSize { get; set; }

		/// <summary>
		/// Gets or sets the resize method this filter should employ during processing.
		/// </summary>
		public ResizeMethod ResizeMethod { get; set; }

		/// <summary>
		/// Gets or sets the anchor location when the resize method is 'Crop'. Ignored otherwise.
		/// </summary>
		public AnchorLocation AnchorLocation { get; set; }

		/// <summary>
		/// Gets or sets the resolution of the output image
		/// </summary>
		public Resolution Resolution { get; set; }

		/// <summary>
		/// Gets or sets the compositing quality.
		/// </summary>
		public CompositingQuality CompositingQuality { get; set; }

		/// <summary>
		/// Gets or sets the interpoliation mode.
		/// </summary>
		public InterpolationMode InterpoliationMode { get; set; }

		/// <summary>
		/// Gets or sets the background colour.
		/// </summary>
		public Color BackgroundColour { get; set; }

        /// <summary>
        /// Whether or not the axis should be constrained. Set null for default behaviour.
        /// </summary>
        public Axis? ConstrainAxis { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ResizeFilter"/> class.
		/// </summary>
		public ResizeFilter(int width, int height)
			: this(width, height, ResizeMethod.KeepAspectRatio)
		{
		}

		/// <summary>
		/// Initialises a new instance of ResizeFilter with a width and a height.
		/// </summary>
		/// <param name="width">The target width of the image</param>
		/// <param name="height">The target height of the image</param>
		/// <param name="method">The resize method to use when resizing</param>
		public ResizeFilter(int width, int height, ResizeMethod method)
			: this(width, height, method, AnchorLocation.Middle)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ResizeFilter"/> class.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="method">The resize method.</param>
		/// <param name="anchorLocation">The anchor location, for when cropping is used.</param>
		public ResizeFilter(int width, int height, ResizeMethod method, AnchorLocation anchorLocation, Axis? constrainAxis = null)
		{
			this.TargetSize = new Size(width, height);
			this.ResizeMethod = method;
			this.AnchorLocation = anchorLocation;
			this.Resolution = Resolution.Screen;
			this.CompositingQuality = CompositingQuality.HighQuality;
			this.InterpoliationMode = InterpolationMode.HighQualityBicubic;
			this.BackgroundColour = Color.White;
            this.ConstrainAxis = constrainAxis;
		}

		#endregion

		/// <summary>
		/// Processes the image.
		/// </summary>
		/// <param name="inputImage">The input image to process</param>
		/// <returns>The resulting image after processing</returns>
		public virtual Image Process(Image inputImage)
		{
			Size bitmapSize = Size.Empty;
			Size outputSize = GetNewSize(inputImage, this.TargetSize, this.ResizeMethod, out bitmapSize);

			Bitmap outputBmp = new Bitmap(bitmapSize.Width, bitmapSize.Height);
			outputBmp.SetResolution(this.Resolution.X, this.Resolution.Y);

			Graphics g = Graphics.FromImage(outputBmp);
			g.CompositingQuality = this.CompositingQuality;
			g.InterpolationMode = this.InterpoliationMode;

			Rectangle destRect = new Rectangle(new Point(0, 0), outputSize);
			Rectangle sourceRect = new Rectangle(0, 0, inputImage.Width, inputImage.Height);

			float outputAspect = (float)outputSize.Width / (float)outputSize.Height;

			if (this.ResizeMethod == ResizeMethod.Crop)
				sourceRect = GetLargestInset(sourceRect, outputAspect, this.AnchorLocation);
			else if (this.ResizeMethod == Filters.ResizeMethod.AspectRatioFill)
			{
				Rectangle bitmapRectangle = new Rectangle(Point.Empty, bitmapSize);

				// Draw a background colour.
				g.FillRectangle(new SolidBrush(this.BackgroundColour), bitmapRectangle);

				// Position the image inside according to the anchor location
				RectangleUtil.PositionRectangle(this.AnchorLocation, bitmapRectangle, ref destRect);
			}

			g.DrawImage(inputImage, destRect, sourceRect, GraphicsUnit.Pixel);

			return outputBmp;
		}

		/// <summary>
		/// Calculates the new size of the image, based on the resize method and the requested size.
		/// The output size may not be the same as the input size if the user has requested
		/// to keep the same aspect ratio.
		/// </summary>
		/// <param name="img">The image to resize</param>
		/// <param name="requestedSize">The requested new image size</param>
		/// <param name="method">The method to employ whilst resizing</param>
		/// <param name="bitmapSize">The recommended size of the output bitmap. Normally the size as the size returned from this function.</param>
		/// <returns>
		/// The actual size that any output image should be, based on the input criteria.
		/// </returns>
		protected virtual Size GetNewSize(Image img, Size requestedSize, ResizeMethod method, out Size bitmapSize)
		{
			Size outputSize = new Size();

			switch (method)
			{
                case ResizeMethod.AspectRatioFill:
                    {
                        float ratio = (float)img.Width / (float)img.Height;

                        outputSize.Width = (int)((float)requestedSize.Height * ratio);
                        outputSize.Height = requestedSize.Height;

                    } break;

				case ResizeMethod.KeepAspectRatio:
                    {
                        float ratio = (float)img.Width / (float)img.Height;

                        if (this.ConstrainAxis.HasValue)
                        {
                            if (this.ConstrainAxis.Value == Axis.Horizontal)
                            {
                                outputSize.Width = (int)(requestedSize.Width);
                                outputSize.Height = (int)((float)requestedSize.Width / ratio);
                            }
                            else
                            {
                                outputSize.Width = (int)((float)requestedSize.Height * ratio);
                                outputSize.Height = requestedSize.Height;
                            }
                        }
                        else
                        {
                            if (ratio > 1)
                            {
                                outputSize.Width = (int)(requestedSize.Width);
                                outputSize.Height = (int)((float)requestedSize.Width / ratio);
                            }
                            else
                            {
                                outputSize.Width = (int)((float)requestedSize.Height * ratio);
                                outputSize.Height = requestedSize.Height;
                            }
                        }
					} break;

				case ResizeMethod.Absolute:
				case ResizeMethod.Crop:
					{
						// The actual size of the output image is going to be exactly what was asked for
						outputSize = requestedSize;
					} break;
			}

			if (method == Filters.ResizeMethod.KeepAspectRatio)
				bitmapSize = outputSize;
			else
				bitmapSize = requestedSize;

			return outputSize;
		}

		// Define other methods and classes here
		/// <summary>
		/// Finds the largest rectangle which can fit inside sourceRect with the desired aspect ratio
		/// </summary>
		/// <param name="sourceRect">The container rectangle</param>
		/// <param name="desiredAspect">The desired aspect ratio</param>
		/// <param name="anchorLocation">The anchor location.</param>
		/// <returns>A Rectangle which represents the largest possible area that should be taken from the source image given the input parameters.</returns>
		public virtual Rectangle GetLargestInset(Rectangle sourceRect, float desiredAspect, AnchorLocation anchorLocation)
		{
			Rectangle destRect = default(Rectangle);

			float sourceAspect = (float)sourceRect.Width / (float)sourceRect.Height;
			float ratioScale = desiredAspect / sourceAspect;

			if (sourceAspect > desiredAspect)
			{
				destRect.Width = (int)((float)sourceRect.Width * ratioScale);
				destRect.Height = sourceRect.Height;
			}
			else
			{
				destRect.Width = sourceRect.Width;
				destRect.Height = (int)((float)sourceRect.Height / ratioScale);
			}

			RectangleUtil.PositionRectangle(anchorLocation, sourceRect, ref destRect);

			return destRect;
		}

	}
}
