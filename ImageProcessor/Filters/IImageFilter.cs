using System.Drawing;

namespace Simplicode.Imaging.Filters
{
	/// <summary>
	/// An interface which defines an image filter.
	/// </summary>
	public interface IImageFilter
	{
		/// <summary>
		/// Performs the processing of an image.
		/// </summary>
		/// <param name="inputImage">The image to perform the processing on.</param>
		/// <returns>The image after it has been processed by this filter.</returns>
		Image Process(Image inputImage);
	}
}
