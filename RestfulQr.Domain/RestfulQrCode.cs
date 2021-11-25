using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulQr.Domain
{
    /// <summary>
    /// Represents a qr code that was generated.
    /// </summary>
    public class RestfulQrCode
    {
        /// <summary>
        /// The unique ID of the qr code
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The type of the QR code
        /// </summary>
        public QrCodeType Type { get; set; }

        /// <summary>
        /// The type of render the QR code was created with
        /// </summary>
        public RenderType RenderType { get; set; }

        /// <summary>
        /// The date this qr code was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The api key that was used to created this qr code
        /// <see cref="ApiKey"/>
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// The name of this file (including extension)
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The model serialized as JSON
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// The public URL that the image can be accessed at
        /// </summary>
        public string PublicUrl { get; set; }
    }
}