using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplicode.Imaging.Filters;
using System.IO;
using System.Drawing;

namespace Simplicode.ImageProcessor.TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			// 1. Simple image resize:
			Simplicode.Imaging.ImageProcessor processor = new Imaging.ImageProcessor();
			
			var resizer = new ResizeFilter(400, 400, ResizeMethod.KeepAspectRatio);
			processor.AddFilter(resizer);
			processor.ProcessImage("testimage.png", "testimage_resized.png");

			// 2. Image conversion
			processor = new Imaging.ImageProcessor();
			//processor.OutputFormat = Imaging.OutputFormat.Jpeg;
			processor.ProcessImage("testimage.png", "testimage_converted.jpg");

			// 2.1 Image processing using streams
			processor = new Imaging.ImageProcessor();

            processor.OutputFormat = Imaging.ImageFormatType.Jpeg;
			processor.JpegCompression = 90L;
			using (var inputStream = new FileStream("testimage.png", FileMode.Open))
			{
				using (var outputStream = new FileStream("testimage_converted_stream.jpg", FileMode.Create))
				{
					processor.ProcessImage(inputStream, outputStream);
				}
			}

			// 2.2 GIF support
			processor = new Imaging.ImageProcessor();
			processor.GifColours = 128;
            processor.OutputFormat = Imaging.ImageFormatType.Gif;
			processor.ProcessImage("testimage.png", "testimage.gif");

			// 3. Cropping
			processor = new Imaging.ImageProcessor();		
			processor.Filters.Add(new ResizeFilter(400, 300, ResizeMethod.Crop, AnchorLocation.TopMiddle));
			processor.ProcessImage("testimage.png", "testimage_cropped_top.png");

			// 4. Edge detection
			processor = new Imaging.ImageProcessor(new EdgeDetectionFilter(10, Color.Red, Color.Yellow));
			processor.ProcessImage("testimage.png", "testimage_edge.png");

			// 5. Chaining
			processor = new Imaging.ImageProcessor();
			processor.Filters.Add(new EdgeDetectionFilter(7));
			processor.Filters.Add(new ResizeFilter(200, 200));
			processor.ProcessImage("testimage.png", "testimage_chained.jpg");

			// 6. Quality changing
			processor = new Imaging.ImageProcessor();
			resizer = new ResizeFilter(400, 400);
			resizer.InterpoliationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
			resizer.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
			processor.Filters.Add(resizer);
			processor.ProcessImage("testimage.png", "testimage_lowquality.jpg");

			// 7. Aspect Ratio Fill
			processor = new Imaging.ImageProcessor();
			resizer = new ResizeFilter(200, 50, ResizeMethod.AspectRatioFill);
			processor.AddFilter(resizer);
			processor.ProcessImage("testimage.png", "testimage_fill.png");

			// 8. Image watermark
			processor = new Imaging.ImageProcessor();
			var watermark = new ImageWatermarkFilter("watermark.png", AnchorLocation.BottomRight);
			watermark.Offset = new Point(-20, -20);
			watermark.Opacity = .3f;

			processor.AddFilter(watermark);
			processor.ProcessImage("testimage.png", "testimage_image_watermarked.png");

			// 9. Text watermark
			processor = new Imaging.ImageProcessor();
			var textWatermark = new TextWatermarkFilter("Copyright (C) Some Company 2011")
			{
				FontStyle = FontStyle.Bold,
				FontName = "Arial",
				FontSize = 24,		
				Offset = new Point(-20, -20)
			};
			textWatermark.Opacity = .5f;
			processor.AddFilter(textWatermark);
			processor.ProcessImage("testimage.png", "testimage_text_watermarked.png");

		}
	}
}
