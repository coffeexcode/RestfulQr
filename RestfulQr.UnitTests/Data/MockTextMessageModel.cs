using RestfulQr.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public class MockTextMessageModel : IMockModel<CreateTextMessageModel>
    {
        public CreateTextMessageModel Invalid()
        {
            return new CreateTextMessageModel
            {
                Body = "body",
            };
        }

        public CreateTextMessageModel Valid()
        {
            return new CreateTextMessageModel
            {
                Body = "body",
                Phone = "123-123-1234",
                SmsEncoding = QRCoder.PayloadGenerator.SMS.SMSEncoding.SMSTO,
                TextMessageType = Domain.TextMessageType.SMS
            };
        }
    }
}
