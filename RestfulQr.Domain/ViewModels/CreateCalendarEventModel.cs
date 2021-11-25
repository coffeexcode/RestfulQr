using System.ComponentModel.DataAnnotations;
using static QRCoder.PayloadGenerator.CalendarEvent;

namespace RestfulQr.Domain.ViewModels
{
    public class CreateCalendarEventModel
    {
        [Required]
        [MaxLength(Sizing.Md)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(Sizing.Lg)]
        public string Description { get; set; }

        [Required]
        [MaxLength(Sizing.Lg)]
        public string Location { get; set; }

        [Required]
        public DateTimeOffset Start { get; set; }

        public DateTimeOffset? End { get; set; }

        [Required]
        public bool AllDay { get; set; }

        [Required]
        public EventEncoding EventEncoding { get; set; }
    }
}
