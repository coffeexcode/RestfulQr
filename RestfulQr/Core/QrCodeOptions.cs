using static QRCoder.QRCodeGenerator;

namespace RestfulQr.Core
{
    public class QrCodeOptions
    {
        public ECCLevel ECCLevel { get; set; }

        public int PixelsPerModule { get; set; }

        public string Dark { get; set; }

        public string Light { get; set; }

        public bool DrawQuietZones { get; set; }
    }
}
