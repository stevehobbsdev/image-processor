using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Simplicode.Imaging.Filters
{
	/// <summary>
	/// Applies a text watermark string to the processed image
	/// </summary>
	public class TextWatermarkFilter : WatermarkFilter
	{
		/// <summary>
		/// Gets or sets the text for the watermark
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the name of the font.
		/// </summary>
		/// <value>
		/// The name of the font.
		/// </value>
		public string FontName { get; set; }

		/// <summary>
		/// Gets or sets the font style.
		/// </summary>
		public FontStyle FontStyle { get; set; }

		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		/// <value>
		/// The size of the font.
		/// </value>
		public int FontSize { get; set; }

		/// <summary>
		/// Gets or sets the text colour.
		/// </summary>
		public Color TextColour { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TextWatermarkFilter"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="textColour">The text colour.</param>
		public TextWatermarkFilter(string text, Color textColour)
			: this(text)
		{
			this.TextColour = textColour;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextWatermarkFilter"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="fontName">Name of the font.</param>
		/// <param name="fontSize">Size of the font.</param>
		/// <param name="anchorLocation">The anchor location.</param>
		/// <param name="style">The font style.</param>
		public TextWatermarkFilter(string text, 
			string fontName = "Arial", 
			int fontSize = 24,
			AnchorLocation anchorLocation = Filters.AnchorLocation.BottomRight,			
			FontStyle style = FontStyle.Regular)
		{
			this.Text = text;
			this.FontName = fontName;
			this.FontSize = fontSize;
			this.AnchorLocation = anchorLocation;			
			this.FontStyle = style;
			this.TextColour = Color.White;
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

				Font font = new Font(this.FontName, this.FontSize, this.FontStyle, GraphicsUnit.Pixel);

				// Position the text according to the size of the string and the specified anchor location.
				Size size = g.MeasureString(this.Text, font).ToSize();

				Rectangle imageArea = new Rectangle(Point.Empty, inputImage.Size);
				Rectangle watermarkArea = new Rectangle(Point.Empty, size);

				RectangleUtil.PositionRectangle(this.AnchorLocation, imageArea, ref watermarkArea);

				watermarkArea.Offset(this.Offset);

				// Mix the specified colour with our opacity value
				Color textColour = Color.FromArgb((int)(this.Opacity * 255.0f), this.TextColour);

				// Draw the text
				g.DrawString(this.Text, font, new SolidBrush(textColour), new PointF((float)watermarkArea.X, (float)watermarkArea.Y));
			}
			
			return inputImage;
		}
	}
}
