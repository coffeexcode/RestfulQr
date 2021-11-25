using RestfulQr.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public class MockWhatsAppMessageModel : IMockModel<CreateWhatsAppMessageModel>
    {
        public CreateWhatsAppMessageModel Invalid()
        {
            return new CreateWhatsAppMessageModel
            {
                Body = "body",
            };
        }

        public CreateWhatsAppMessageModel Valid()
        {
            return new CreateWhatsAppMessageModel
            {
                Body = "body",
                Phone = "123-123-1234"
            };
        }
    }
}
