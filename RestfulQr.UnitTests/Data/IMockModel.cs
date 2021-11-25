using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.UnitTests.Data
{
    public interface IMockModel<T>
    {
        public T Valid();

        public T Invalid();
    }
}
