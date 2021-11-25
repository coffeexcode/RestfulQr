using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static QRCoder.QRCodeGenerator;

namespace RestfulQr.Domain
{
    public class QrCodeRenderOptions
    {
        public ECCLevel EccLevel { get; set; }

        public string LightColour { get; set; }

        public string DarkColour { get; set; }

        public bool DrawQuietZones { get; set; }

        public int PixelsPerModule { get; set; }

        public RenderType RenderType { get; set; }

        public bool Preview { get; set; }
    }
}