using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Simplicode.Imaging.Filters;
using System.Collections.Generic;
using Simplicode.Imaging.ProviderModel;
using System.Reflection;
using System.Linq;

namespace Simplicode.Imaging
{
	/// <summary>
	/// Processes one or more image with one or more image filters
	/// </summary>
	public class ImageProcessor : IDisposable
	{
		#region  Properties

		/// <summary>
		/// Gets the collection of filters which are currently used by this processor.
		/// </summary>
		public FilterCollection Filters
		{
			get
			{
				if (_filters == null)
					_filters = new FilterCollection();
				return _filters;
			}
		}
		private FilterCollection _filters = null;

		/// <summary>
		/// Gets the index of the current filter being used in the stack.
		/// </summary>
		public int CurrentFilterIndex
		{
			get { return _currFilterIndex; }
			private set { _currFilterIndex = value; }
		}
		private int _currFilterIndex = 0;

		/// <summary>
		/// Gets the current filter in use by the processor.
		/// </summary>
		public IImageFilter CurrentFilter
		{
			get { return _current; }
			private set { _current = value; }
		}
		private IImageFilter _current = null;

		/// <summary>
		/// Gets or sets the output format of the image.
		/// </summary>
		public ImageFormatType OutputFormat
		{
			get { return _format; }
			set { _format = value; }
		}
		private ImageFormatType _format = ImageFormatType.Default;

		/// <summary>
		/// Gets or sets the value for Jpeg compression. The default is 80. Only applies if saving
		/// with an output format of Jpeg.
		/// </summary>
		/// <remarks>This is usually a number between 10 and 100, where 100 is the best quality 
		/// (but largest) output image.</remarks>
		public long JpegCompression
		{
			get { return _jpegCompression; }
			set { _jpegCompression = value; }
		}
		private long _jpegCompression = 80L;

		/// <summary>
		/// Gets or sets the amount of colours to use when saving GIF images.
		/// </summary>
		public short GifColours
		{
			get { return _gifColours; }
			set { _gifColours = value; }
		}
		private short _gifColours = 256;

		/// <summary>
		/// Gets the provider factory.
		/// </summary>
		public ImageProviderFactory ProviderFactory
		{
			get { return _providerFactory; }
		}
		private ImageProviderFactory _providerFactory = new ImageProviderFactory();

		#endregion


		#region Events

		/// <summary>
		/// Occurs immediately before an image gets processed by the entire stack.
		/// </summary>
		public event ImageEventHandler ImagePreFiltered;

		/// <summary>
		/// Occurs when an image has passed through a filtering stage.
		/// </summary>
		public event ImageEventHandler ImageFiltered;

		/// <summary>
		/// Occurs just before the image is passed through the filter stack
		/// </summary>
		public event ProcessEventHandler BeginProcessing;

		/// <summary>
		/// Occurs at the point when an image has finished being processed, before the output has been saved.
		/// </summary>
		public event ImageEventHandler EndProcessing;

		#endregion


		#region Constructors

		/// <summary>
		/// Initialises a new instance of ImageProcessor
		/// </summary>
		public ImageProcessor()
		{
		}

		/// <summary>
		/// Initialises a new instance of ImageProcessor with an initial image filter to use.
		/// </summary>
		/// <param name="filter">The initial image filter to use when processing.</param>
		public ImageProcessor(IImageFilter filter)
		{
			Filters.Add(filter);
		}

		/// <summary>
		/// Initialises a new instance of ImageProcessor with a collection of filters to use.
		/// </summary>
		/// <param name="filters">A collection of filters to use for processing.</param>
		public ImageProcessor(IEnumerable<IImageFilter> filters)
		{
			this.Filters.Clear();
			this.Filters.AddRange(filters);
		}

		#endregion


		#region Methods

		/// <summary>
		/// Adds a new filter to the filter processing queue.
		/// </summary>
		/// <param name="filter">The filter to be added.</param>
		/// <remarks>Images are processed by filters in the order that they are added to the
		/// filter queue.</remarks>
		public void AddFilter(IImageFilter filter)
		{
			Filters.Add(filter);
		}

		/// <summary>
		/// Clears the current filter queue.
		/// </summary>
		public void ClearFilters()
		{
			Filters.Clear();
		}

		/// <summary>
		/// Processes an image using the filters which have been added to the filter collection.
		/// </summary>
		/// <param name="inputImage">The real path to the input image.</param>
		/// <param name="outputImage">The real path to the resulting image after processing.</param>
		public void ProcessImage(string inputImage, string outputImage)
		{
			using (Stream inputStream = new FileStream(inputImage, FileMode.Open))
			{
				using (Stream outputFileStream = new FileStream(outputImage, FileMode.Create))
				{
					ImageFormatType inputFormat = Util.DiscoverOutputFormatFromFile(inputImage);

					// Try and discover the output format from the extension, if one hasn't already been set.
					if (OutputFormat == ImageFormatType.Default)
					{
						OutputFormat = Util.DiscoverOutputFormatFromFile(outputImage);
					}

					ProcessImage(inputStream, outputFileStream, inputFormat, OutputFormat);
				}
			}
		}

		/// <summary>
		/// Processes an image using the filters in the filter collection, setting the image
		/// in an input stream.
		/// </summary>
		/// <param name="inputStream">The input stream containing image data.</param>
		/// <param name="outputStream">The stream to write the resulting image to</param>
		public void ProcessImage(Stream inputStream, Stream outputStream, ImageFormatType inputImageFormat = ImageFormatType.Default, ImageFormatType outputImageFormat = ImageFormatType.Default)
		{
			IImageProvider inputProvider = this.ProviderFactory.FindBestProvider(inputImageFormat);
			IImageProvider outputProvider = this.ProviderFactory.FindBestProvider(outputImageFormat);

			IEnumerable<Image> inputImages = inputProvider.Read(inputStream);
			List<Image> outputImages = new List<Image>();

			foreach (var image in inputImages)
			{
				outputImages.Add(ProcessImage(image));
			}

			outputProvider.Write(outputImages, outputStream, outputImageFormat, this.JpegCompression);
		}

		/// <summary>
		/// Processes an initial image using the filters in the filter collection
		/// </summary>
		/// <param name="input">The input image object to process</param>
		/// <returns></returns>
		public Image ProcessImage(Image input)
		{
			InitialiseEventHooks();

			return ProcessFilters(input);
		}

		/// <summary>
		/// Performs the processing of the image filter queue.
		/// </summary>
		/// <param name="input">The input image to perform the processing on.</param>
		/// <returns>The image after it has been processed by the entire filter stack.</returns>
		protected virtual Image ProcessFilters(Image input)
		{
			Image img = input;

			try
			{
				BeginProcessing(this, new ProcessorEventArgs());

				foreach (IImageFilter filter in Filters)
				{
					CurrentFilter = filter;

					ImagePreFiltered(this, new ImageEventArgs(img, CurrentFilter));

					img = filter.Process(img);

					ImageFiltered(this, new ImageEventArgs(img, CurrentFilter));

				}

				CurrentFilter = null;
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				EndProcessing(this, new ImageEventArgs(img, null));
			}

			return img;
		}



		/// <summary>
		/// Sets up our local instance event hooks
		/// </summary>
		private void InitialiseEventHooks()
		{
			// Create event hooks
			this.ImageFiltered += new ImageEventHandler(OnImageFiltered);
			this.ImagePreFiltered += new ImageEventHandler(OnImagePreFiltered);
			this.BeginProcessing += new ProcessEventHandler(OnBeginProcessing);
			this.EndProcessing += new ImageEventHandler(OnEndProcessing);
		}

		#endregion


		#region Event Handlers

		/// <summary>
		/// Handles the ImageFiltered event, which is fired when an image passes through a filtering stage.
		/// </summary>
		/// <param name="sender">The processor which is processing the image.</param>
		/// <param name="e">The event arguments containing the image.</param>
		protected virtual void OnImageFiltered(object sender, ImageEventArgs e)
		{
		}

		/// <summary>
		/// Handles the ImagePreProcessed event, which occurs immediately before an image gets 
		/// processed by a filter.
		/// </summary>
		/// <param name="sender">The processor which is processing this image.</param>
		/// <param name="e">The event arguments containing the image.</param>
		protected virtual void OnImagePreFiltered(object sender, ImageEventArgs e)
		{
		}

		/// <summary>
		/// Handles the EndProcessing event, which occurs when the processor has finishing processing
		/// the filter stack, before the image has been saved.
		/// </summary>
		/// <param name="sender">The instance of the image processor.</param>
		/// <param name="e">The processor event arguments.</param>
		protected virtual void OnEndProcessing(object sender, ImageEventArgs e)
		{
		}

		/// <summary>
		/// Handles the BeginProcessing event, which occurs immediately before the processor begins
		/// processing the filter stack.
		/// </summary>
		/// <param name="sender">The instance of the image processor</param>
		/// <param name="e">The processor event arguments.</param>
		protected virtual void OnBeginProcessing(object sender, ProcessorEventArgs e)
		{
		}

		#endregion


		#region Convenience Resizing

		/// <summary>
		/// Resizes the image.
		/// </summary>
		/// <param name="inputFilename">The input filename.</param>
		/// <param name="outputFilename">The output filename.</param>
		/// <param name="width">The desired width of the output image.</param>
		/// <param name="height">The desired height of the output image.</param>
		/// <param name="method">The resize calculation method</param>
		/// <param name="anchorLocation">The anchor location.</param>
		/// <param name="jpegQuality">The JPEG quality.</param>
		/// <param name="outputFormat">The output format.</param>
		public static void ResizeImage(string inputFilename, 
			string outputFilename, 
			int width, 
			int height, 
			ResizeMethod method = ResizeMethod.KeepAspectRatio, 
			AnchorLocation anchorLocation = AnchorLocation.Middle,
			long jpegQuality = 80L, 
			ImageFormatType outputFormat = ImageFormatType.Default)
		{
			ImageProcessor processor = new ImageProcessor(new ResizeFilter(width, height, method, anchorLocation));
			ResizeFilter filter = new ResizeFilter(width, height, method, anchorLocation);

			processor.JpegCompression = jpegQuality;
			processor.OutputFormat = outputFormat;

			processor.ProcessImage(inputFilename, outputFilename);
		}

		#endregion


		#region IDisposable Members

		/// <summary>
		/// Cleans up any memory taken by the image processor.
		/// </summary>
		public void Dispose()
		{
		}

		#endregion
	}
}
