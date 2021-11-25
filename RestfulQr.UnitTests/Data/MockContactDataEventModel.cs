using RestfulQr.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public class MockContactDataEventModel : IMockModel<CreateContactDataEventModel>
    {
        public CreateContactDataEventModel Invalid()
        {
            return new CreateContactDataEventModel
            {
                FirstName = "invalid"
            };
        }

        public CreateContactDataEventModel Valid()
        {
            return new CreateContactDataEventModel
            {
                FirstName = "test",
                LastName = "tester",
                AddressOrder = QRCoder.PayloadGenerator.ContactData.AddressOrder.Default,
                Birthday = DateTime.Now,
                City = "city",
                Company = "company",
                Country = "country",
                Email = "email",
                HouseNumber = "123",
                MobilePhone = "mobilePhone",
                Nickname = "nickname",
                Note = "note",
                OutputType = QRCoder.PayloadGenerator.ContactData.ContactOutputType.MeCard,
                Phone = "phone",
                PostalZip = "postalZip",
                ProvinceState = "provinceState",
                Street = "street",
                Website = "https://website.com",
                WorkPhone = "workPhone",
            };
        }
    }
}
