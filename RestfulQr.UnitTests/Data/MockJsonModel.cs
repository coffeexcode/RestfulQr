using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public class MockJsonModel : IMockModel<object>
    {
        public object Valid()
        {
            return new
            {
                data = "mocked",
                name = "test",
                date = DateTime.Now,
            };
        }

        public object Invalid()
        {
            return new { };
        }
    }
}
