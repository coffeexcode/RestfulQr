using System.ComponentModel.DataAnnotations;
using static QRCoder.PayloadGenerator.WiFi;

namespace RestfulQr.Domain.ViewModels
{
    public class CreateWifiModel
    {
        #region Required
        [Required]
        [MaxLength(Sizing.Name)]
        public string Ssid { get; set; }

        [Required]
        [MaxLength(Sizing.Name)]
        public string Password { get; set; }

        [Required]
        public Authentication Authentication { get; set; }
        #endregion

        public bool Hidden { get; set; } = false;
    }
}
