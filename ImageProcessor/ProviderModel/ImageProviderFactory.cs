using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplicode.Imaging.ProviderModel
{
	public class ImageProviderFactory : Simplicode.Imaging.ProviderModel.IImageProviderFactory
	{
		/// <summary>
		/// Gets the image providers, indexed by output format
		/// </summary>
		public Dictionary<ImageFormatType, IImageProvider> ImageProviders { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageProviderFactory"/> class.
		/// </summary>
		public ImageProviderFactory()
		{
			this.ImageProviders = BuildProviderIndex();
		}

		/// <summary>
		/// Finds the best provider.
		/// </summary>
		/// <param name="format">The format.</param>
		public IImageProvider FindBestProvider(ImageFormatType format)
		{
			if (ImageProviders.ContainsKey(format))
				return ImageProviders[format];
			else
				return ImageProviders[ImageFormatType.Default];
		}

		/// <summary>
		/// Builds an index of the available providers for each format
		/// </summary>
		protected Dictionary<ImageFormatType, IImageProvider> BuildProviderIndex()
		{
			var providerTypes = (from t in this.GetType().Assembly.GetTypes()
								 where t.GetInterfaces().Any(i => i.Name == typeof(IImageProvider).Name)
								 select t).ToList();

			Dictionary<ImageFormatType, IImageProvider> index = new Dictionary<ImageFormatType, IImageProvider>();

			foreach (var type in providerTypes)
			{
				object[] attributes = type.GetCustomAttributes(typeof(ImageProviderAttribute), true);

				if (attributes != null)
				{
					ImageProviderAttribute providerAttribute = attributes[0] as ImageProviderAttribute;

					if (index.ContainsKey(providerAttribute.TargetFormat))
						throw new InvalidOperationException("Duplicate image provider found: tried to index type " + type.Name + " but a similar provider was already indexed");

					IImageProvider provider = Activator.CreateInstance(type) as IImageProvider;
					index.Add(providerAttribute.TargetFormat, provider);
				}
			}

			return index;
		}
	}
}
