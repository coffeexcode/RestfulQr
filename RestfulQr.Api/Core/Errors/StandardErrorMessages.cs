namespace RestfulQr.Api.Core.Errors
{
    public static class StandardErrorMessages
    {
        public static class Requests
        {
            public static readonly string UnableToRenderQrCode = "Unable to render QR code";
            public static class Text
            {
                public static readonly string MustContainContent = "Request body must contain property 'content'";
                public static readonly string ContentMustNotBeEmpty = "Property 'content' must not be empty";
            }
        }


    }
}
