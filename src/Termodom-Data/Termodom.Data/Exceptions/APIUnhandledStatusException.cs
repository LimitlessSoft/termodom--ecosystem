using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Termodom.Data.Exceptions
{
    /// <summary>
    /// Exception koji se koristi prilikom obrade status kod-a sa apija
    /// i dobija se kada API vrati status kod koji mi nismo obradili
    /// </summary>
    public class APIUnhandledStatusException : Exception
    {
        public APIUnhandledStatusException(System.Net.HttpStatusCode statusCode) : base($"[ API Unhandled status code: {Convert.ToInt32(statusCode)} ]") { }
    }
}
