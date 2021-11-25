using System.ComponentModel.DataAnnotations;

namespace RestfulQr.Domain.ViewModels
{
    public class CreateWhatsAppMessageModel
    {
        [Required]
        [MaxLength(Sizing.Phone)]
        public string Phone { get; set; }

        [MaxLength(Sizing.Lg)]
        public string Body { get; set; }
    }
}
