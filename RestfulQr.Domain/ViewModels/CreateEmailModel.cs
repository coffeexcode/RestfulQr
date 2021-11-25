using System.ComponentModel.DataAnnotations;
using static QRCoder.PayloadGenerator.Mail;

namespace RestfulQr.Domain.ViewModels
{
    public class CreateEmailModel
    {
        #region Required
        [Required]
        [MaxLength(Sizing.Email)]
        public string Recipient { get; set; }

        [Required]
        public MailEncoding Encoding { get; set; }
        #endregion

        [Required]
        [MaxLength(Sizing.Md)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(Sizing.Xl)]
        public string Body { get; set; }
    }
}
