using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public class MockTextModel : IMockModel<object>
    {
        public object Valid()
        {
            return new
            {
                content = "Mocked"
            };
        }

        public object Invalid()
        {
            return new
            {
                property = "invalid"
            };
        }
    }
}
