using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ubiety.Dns.Core;

namespace TDOffice_v2.TDWeb
{
    public class Korisnik
    {
        public static object List()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/api/Korisnik/List");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Program.APIToken);

            HttpClient client = new HttpClient();
            return client.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
        }
    }
}
