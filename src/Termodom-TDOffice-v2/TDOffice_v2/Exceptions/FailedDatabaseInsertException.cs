using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Exceptions
{
    public class FailedDatabaseInsertException : Exception
    {
        private string _message { get; set; }
        public override string Message { get => _message; }

        public FailedDatabaseInsertException()
        {

        }
        public FailedDatabaseInsertException(string message)
        {
            _message = message;
        }

        public override string ToString()
        {
            return "TDOffice.FailedDatabaseInsert: " + _message;
        }
    }
}
