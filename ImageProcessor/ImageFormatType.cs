
namespace Simplicode.Imaging
{
	/// <summary>
	/// Describes the possible image output formats that can be chosen
	/// </summary>
	public enum ImageFormatType
	{
		/// <summary>
		/// With the default setting, the output format will usually be determined by some other factor, like the file extension.
		/// </summary>
		Default = 0,

#pragma warning disable 1591
		Bmp = 1,
		Jpeg = 2,
		Gif = 3,
		Png = 4,
		Tiff = 5
#pragma warning restore 1591
	}
}
