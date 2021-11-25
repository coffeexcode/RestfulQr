using System.ComponentModel.DataAnnotations;
using static QRCoder.PayloadGenerator.MMS;
using static QRCoder.PayloadGenerator.SMS;

namespace RestfulQr.Domain.ViewModels
{
    public class CreateTextMessageModel
    {
        #region Required
        [Required]
        [MaxLength(Sizing.Phone)]
        public string Phone { get; set; }

        [Required]
        public TextMessageType TextMessageType { get; set; }
        #endregion

        public SMSEncoding SmsEncoding { get; set; }

        public MMSEncoding MmsEncoding { get; set; }

        [MaxLength(Sizing.Lg)]
        public string Body { get; set; }
    }
}
