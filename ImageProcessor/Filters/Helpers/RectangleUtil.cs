using System.Drawing;

namespace Simplicode.Imaging.Filters
{
	internal sealed class RectangleUtil
	{
		public static void PositionRectangle(AnchorLocation anchorLocation, Rectangle sourceRect, ref Rectangle destRect)
		{
			// Position the rectangle based on the anchor location
			switch (anchorLocation)
			{
				// Top -------------------------

				case Filters.AnchorLocation.TopLeft:
					destRect.X = destRect.Y = 0;
					break;

				case Filters.AnchorLocation.TopMiddle:
					destRect.X = (sourceRect.Width - destRect.Width) / 2;
					destRect.Y = 0;
					break;

				case Filters.AnchorLocation.TopRight:
					destRect.X = sourceRect.Width - destRect.Width;
					destRect.Y = 0;
					break;

				// Middle -------------------------

				case Filters.AnchorLocation.MiddleLeft:
					destRect.X = 0;
					destRect.Y = (sourceRect.Height - destRect.Height) / 2;
					break;

				case Filters.AnchorLocation.Middle:
					destRect.X = (sourceRect.Width - destRect.Width) / 2;
					destRect.Y = (sourceRect.Height - destRect.Height) / 2;
					break;

				case Filters.AnchorLocation.MiddleRight:
					destRect.X = sourceRect.Width - destRect.Width;
					destRect.Y = (sourceRect.Height - destRect.Height) / 2;
					break;

				// Bottom

				case Filters.AnchorLocation.BottomLeft:
					destRect.X = 0;
					destRect.Y = sourceRect.Height - destRect.Height;
					break;

				case Filters.AnchorLocation.BottomMiddle:
					destRect.X = (sourceRect.Width - destRect.Width) / 2;
					destRect.Y = sourceRect.Height - destRect.Height;
					break;

				case Filters.AnchorLocation.BottomRight:
					destRect.X = sourceRect.Width - destRect.Width;
					destRect.Y = sourceRect.Height - destRect.Height;
					break;
			}

		}
	}
}
