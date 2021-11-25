using System.ComponentModel.DataAnnotations;
using static QRCoder.PayloadGenerator.Geolocation;

namespace RestfulQr.Domain.ViewModels
{
    public class CreateGeoLocationModel
    {
        [Required]
        [MaxLength(Sizing.Geolocation)]
        public string Latitude { get; set; }

        [Required]
        [MaxLength(Sizing.Geolocation)]
        public string Longitude { get; set; }

        [Required]
        public GeolocationEncoding Encoding { get; set; }
    }
}
