using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplicode.Imaging.Filters
{
	/// <summary>
	/// The method to use when resizing
	/// </summary>
	public enum ResizeMethod
	{
		/// <summary>
		/// The image is resized to the target resolution regardless of whether or not the
		/// resulting image is distorted.
		/// </summary>
		Absolute = 0,

		/// <summary>
		/// Tries to resize the image to the target resolution, but alters it to keep the image
		/// to it's original aspect ratio.
		/// </summary>
		KeepAspectRatio = 1,

		/// <summary>
		/// Resizes the image with it's correct aspect ratio, placing it within an solid-color image which is sized
		/// to the target resolution.
		/// </summary>
		AspectRatioFill = 2,

		/// <summary>
		/// Crops the image at the requested size, filling out the area with the source image but keeps the original aspect ratio.
		/// </summary>
		Crop = 3
	}
}
