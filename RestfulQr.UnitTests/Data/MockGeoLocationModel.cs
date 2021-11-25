using RestfulQr.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public class MockGeoLocationModel : IMockModel<CreateGeoLocationModel>
    {
        public CreateGeoLocationModel Invalid()
        {
            return new CreateGeoLocationModel
            {
                Latitude = "123.45",
            };
        }

        public CreateGeoLocationModel Valid()
        {
            return new CreateGeoLocationModel
            {
                Latitude = "123.45",
                Longitude = "54.321",
                Encoding = QRCoder.PayloadGenerator.Geolocation.GeolocationEncoding.GoogleMaps
            };
        }
    }
}
