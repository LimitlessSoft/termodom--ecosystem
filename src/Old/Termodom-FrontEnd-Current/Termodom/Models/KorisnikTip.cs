using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Termodom.Models
{
	public static class KorisnikTip
	{
		//Korisnik = 0,
		//Administrator = 1

		public static int? Get(int korisnikID)
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Post,
				Program.BaseAPIUrl + "/Webshop/Korisnik/Tip/Get?korisnikID=" + korisnikID
			);
			request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"bearer",
				Program.APIToken
			);
			HttpClient client = new HttpClient();
			HttpResponseMessage response = client.Send(request);

			return response.StatusCode == System.Net.HttpStatusCode.OK
				? Convert.ToInt32(response.Content.ReadAsStringAsync())
				: null;
		}
	}
}
