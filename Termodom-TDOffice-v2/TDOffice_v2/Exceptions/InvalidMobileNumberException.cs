using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Exceptions
{
    public class InvalidMobileNumberException : Exception
    {
        public string RawNumber { get; set; }
    }
}
