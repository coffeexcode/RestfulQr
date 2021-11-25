using System.ComponentModel.DataAnnotations;

namespace RestfulQr.Domain.ViewModels
{
    public class CreateBookmarkModel
    {
        [Required]
        [MaxLength(Sizing.Url)]
        public string Url { get; set; }

        [Required]
        [MaxLength(Sizing.Xl)]
        public string Title { get; set; }
    }
}
