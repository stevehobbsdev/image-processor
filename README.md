An image processing wrapper around GDI+, allowing you to apply one or more filters against an image source.

Out-of-the-box support:

* Conversion from one image type to another
* Image resizing and various strategies for resolving aspect ratio
* Edge detection
* GIF support
* Chaining filters together to perform complex operations on a single image

Filters can be stacked and queued so that they run one after the other in a process queue. The processor can accept filenames, streams or a GDI image to perform the processing on.

Roadmap:

* More comprehensive filter support for blurring, sharpening, watermarking, inverting, and more!
* Asynchronous processing support
* Batch processing

###Available on Nuget

Install through the Nuget UI inside Visual Studio, or through the Package Manager Console:
`$ install-package Simplicode.ImageProcessor`
