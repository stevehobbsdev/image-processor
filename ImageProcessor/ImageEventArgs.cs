using System;
using System.Drawing;
using Simplicode.Imaging.Filters;

namespace Simplicode.Imaging
{
	/// <summary>
	/// A specialization of EventArgs which are used by the image processor when raising events.
	/// </summary>
	public class ImageEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the image that this event is concerned with. 
		/// </summary>
		public Image Image
		{
			get { return _image; }
			private set { _image = value; }
		}
		private Image _image = null;

		/// <summary>
		/// Gets the filter used to process this image.
		/// </summary>
		public IImageFilter Filter
		{
			get { return _Filter; }
			private set { _Filter = value; }
		}
		private IImageFilter _Filter = null;

		/// <summary>
		/// Initialises a new instance of ImageEventArgs with the associated image.
		/// </summary>
		/// <param name="image">The image that this event is based around.</param>
		/// <param name="filter">The filter currently being used to process this image.</param>
		public ImageEventArgs(Image image, IImageFilter filter)
		{
			Image = image;
			Filter = filter;
		}

	}
}
