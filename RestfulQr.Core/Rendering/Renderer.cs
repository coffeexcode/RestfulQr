using RestfulQr.Domain;

namespace RestfulQr.Core.Rendering
{
    public class Renderer : IQrCodeRenderer
    {
        private readonly QrCodeDataFactory dataFactory;
        private readonly QrCodeImageRenderer imageRenderer;
        public Renderer(QrCodeRenderOptions renderOptions)
        {
            dataFactory = new QrCodeDataFactory(renderOptions);
            imageRenderer = new QrCodeImageRenderer(renderOptions);
        }

        public async Task<byte[]?> RenderAsync(QrCodeType type, object model)
        {
            var data = await dataFactory.CreateAsync(type, model);

            if (data == null) {
                return null;
            }

            var qrCodeMemorySpan = await imageRenderer.RenderAsync(data);

            return qrCodeMemorySpan;
        }
    }
}