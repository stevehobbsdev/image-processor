using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplicode.Imaging.ProviderModel
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	public class ImageProviderAttribute : Attribute
	{
		/// <summary>
		/// Gets a value which represents the target format for this image provider
		/// </summary>
		public readonly ImageFormatType TargetFormat;

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageProviderAttribute"/> class.
		/// </summary>
		/// <param name="targetFormat">The target format.</param>
		public ImageProviderAttribute(ImageFormatType targetFormat)
		{
			this.TargetFormat = targetFormat;
		}
	}
}
