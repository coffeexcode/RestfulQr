using RestfulQr.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public class MockUrlModel : IMockModel<CreateUrlModel>
    {
        public CreateUrlModel Invalid()
        {
            return new CreateUrlModel
            {

            };
        }

        public CreateUrlModel Valid()
        {
            return new CreateUrlModel
            {
                Url = "https://website.com"
            };
        }
    }
}
