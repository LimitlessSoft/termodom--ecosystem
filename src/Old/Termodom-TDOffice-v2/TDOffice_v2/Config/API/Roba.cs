using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TDOffice_v2.API
{
	public class Roba
	{
		public int ID { get; set; }
		public string KatBr { get; set; }
		public string Naziv { get; set; }
		public string JM { get; set; }

		public void Update()
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Post,
				Program.BaseAPIUrl + "/api/Roba/Update"
			);
			request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"bearer",
				Program.APIToken
			);
			request.Content = System.Net.Http.Json.JsonContent.Create<Roba>(this);

			HttpClient client = new HttpClient();
			HttpResponseMessage response = client.SendAsync(request).Result;

			if (response.StatusCode != System.Net.HttpStatusCode.OK)
				throw new Exception("API Error: " + response.StatusCode);
		}

		public static Roba Get(int id)
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Get,
				Program.BaseAPIUrl + "/api/Roba/Get?id=" + id
			);
			request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"bearer",
				Program.APIToken
			);

			HttpClient client = new HttpClient();
			HttpResponseMessage response = client.SendAsync(request).Result;

			return JsonConvert.DeserializeObject<Roba>(response.Content.ReadAsStringAsync().Result);
		}

		public static Task<Roba> GetAsync(int id)
		{
			return Task.Run(() =>
			{
				return Get(id);
			});
		}

		public static List<Roba> List()
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Get,
				Program.BaseAPIUrl + "/api/Roba/List"
			);
			request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"bearer",
				Program.APIToken
			);

			HttpClient client = new HttpClient();
			HttpResponseMessage response = client.SendAsync(request).Result;

			return JsonConvert.DeserializeObject<List<Roba>>(
				response.Content.ReadAsStringAsync().Result
			);
		}

		public static Task<List<Roba>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}
	}
}
