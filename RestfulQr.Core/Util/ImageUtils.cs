using System.Drawing.Imaging;
using RestfulQr.Domain;

namespace RestfulQr.Core.Util
{
    /// <summary>
    /// Statically available methods for dealing with images
    /// </summary>
    public static class ImageUtils
    {
        /// <summary>
        /// Gets a MimeType / ContentType string based on a provided file extension.
        /// </summary>
        /// <param name="extension">The extension to find</param>
        /// <returns>
        /// The mimetype for the extension, otherwise throws a <see cref="NotSupportedException"/>
        /// </returns>
        public static string GetMimeTypeByExtension(string extension)
        {
            return extension switch
            {
                "bmp" => "image/bmp",
                "png" => "image/png",
                "jpeg" or "jpg" => "image/jpeg",
                "svg" => "image/svg+xml",
                _ => throw new NotSupportedException($"The extension '{extension}' is not supported"),
            };
        }

        public static string GetFileExtensionFromImageType(RenderType renderType)
        {
            return renderType switch
            {
                RenderType.Bmp => "bmp",
                RenderType.Png => "png",
                RenderType.Jpeg => "jpeg",
                RenderType.Svg => "svg",
                _ => throw new Exception($"Invalid renderType: {renderType}")
            };
        }

        public static ImageFormat GetImageFormat(RenderType renderType)
        {
            return renderType switch
            {
                RenderType.Bmp => ImageFormat.Bmp,
                RenderType.Png => ImageFormat.Png,
                RenderType.Jpeg => ImageFormat.Jpeg,
                _ => throw new Exception($"Invalid render type: {renderType}"),
            };
        }
    }
}