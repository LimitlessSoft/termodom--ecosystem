using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Termodom.Data.Exceptions
{
    /// <summary>
    /// Exception koji se koristi prilikom obrade status kod-a sa api-ja
    /// kada dobijemo status kod 500
    /// </summary>
    public class APIServerException : Exception
    {
        public APIServerException() : base("[ API Error 500 ]")
        {

        }
        public APIServerException(string message) : base("[ API Error 500 ] " + message) { }
    }
}
