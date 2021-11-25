using RestfulQr.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public class MockWifiModel : IMockModel<CreateWifiModel>
    {
        public CreateWifiModel Invalid()
        {
            return new CreateWifiModel
            {
                Hidden = false,
            };
        }

        public CreateWifiModel Valid()
        {
            return new CreateWifiModel
            {
                Ssid = "mocked",
                Password = "mocked",
                Hidden = false,
                Authentication = QRCoder.PayloadGenerator.WiFi.Authentication.WEP
            };
        }
    }
}
