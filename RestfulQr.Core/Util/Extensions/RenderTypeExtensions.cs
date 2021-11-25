using RestfulQr.Domain;

namespace RestfulQr.Core.Util.Extensions
{
    public static class RenderTypeExtensions
    {
        public static string ToMimeType(this RenderType type)
        {
            return type switch
            {
                RenderType.Bmp => "image/bmp",
                RenderType.Png => "image/png",
                RenderType.Jpeg => "image/jpeg",
                RenderType.Svg => "image/svg+xml",
                _ => throw new ArgumentException($"Invalid render type: {type}")
            };
        }
    }
}