using System;
namespace Simplicode.Imaging.ProviderModel
{
	public interface IImageProviderFactory
	{
		/// <summary>
		/// Finds the best provider for the given image format.
		/// </summary>
		/// <param name="format">The format.</param>
		IImageProvider FindBestProvider(ImageFormatType format);
	}
}
