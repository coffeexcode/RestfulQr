using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulQr.Core.Exceptions
{
    /// <summary>
    /// Exception for when a database entry was not found
    /// </summary>
    public class EntryNotFoundException : Exception
    {
        public EntryNotFoundException(string message) : base(message)
        {

        }
    }
}
