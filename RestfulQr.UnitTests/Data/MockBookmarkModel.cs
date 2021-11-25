using RestfulQr.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public class MockBookmarkModel : IMockModel<CreateBookmarkModel>
    {
        public CreateBookmarkModel Invalid()
        {
            return new CreateBookmarkModel
            {
                Title = "mocked"
            };
        }

        public CreateBookmarkModel Valid()
        {
            return new CreateBookmarkModel
            {
                Title = "mocked",
                Url = "https://website.com"
            };
        }
    }
}
