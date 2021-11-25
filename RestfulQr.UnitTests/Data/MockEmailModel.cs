using RestfulQr.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public class MockEmailModel : IMockModel<CreateEmailModel>
    {
        public CreateEmailModel Invalid()
        {
            return new CreateEmailModel
            {
                Subject = "subject"
            };
        }

        public CreateEmailModel Valid()
        {
            return new CreateEmailModel
            {
                Subject = "subject",
                Body = "body",
                Encoding = QRCoder.PayloadGenerator.Mail.MailEncoding.MAILTO,
                Recipient = "recipient"
            };
        }
    }
}
