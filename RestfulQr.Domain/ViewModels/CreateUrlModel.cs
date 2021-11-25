using System.ComponentModel.DataAnnotations;

namespace RestfulQr.Domain.ViewModels
{
    public class CreateUrlModel
    {
        [Required]
        [MaxLength(Sizing.Url)]
        public string Url { get; set; }
    }
}
