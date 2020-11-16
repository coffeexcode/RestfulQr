using RestfulQr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulQr.ViewModels
{
    /// <summary>
    /// Result of creating a qr code
    /// </summary>
    public class CreateQrCodeResult
    {
        private CreateQrCodeResult() { }

        /// <summary>
        /// Whether the operation was successful
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Any errors that occured during the operation
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// The created qr code entity that was created
        /// <see cref="CreatedQrCode"/>
        /// </summary>
        public CreatedQrCode CreatedQrCode { get; set; }

        /// <summary>
        /// The data that was encoded in the created qr code
        /// </summary>
        public object Data { get; set; }

        public static CreateQrCodeResult Success(CreatedQrCode qrCode)
        {
            return new CreateQrCodeResult
            {
                Succeeded = true,
                CreatedQrCode = qrCode
            };
        }

        public static CreateQrCodeResult Failed(string message)
        {
            return new CreateQrCodeResult
            {
                Succeeded = false,
                Errors = new List<string>
                {
                    message
                }
            };
        }
    }
}
