using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Termodom.API;

namespace Termodom.Models
{
	public class Roba
	{
		public int ID { get; set; }
		public string KatBr { get; set; }
		public string Naziv { get; set; }
		public string JM { get; set; }

		public static string Insert(string katBr, string naziv, string jm)
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Post,
				Program.BaseAPIUrl
					+ "/api/Roba/Insert?katBr="
					+ katBr
					+ "&naziv="
					+ naziv
					+ "&jm="
					+ jm
			);
			APIRequestFailedLog failedLog = null;
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.Created)
				return response.Content.ReadAsStringAsync().Result;
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else
				throw new APIResponseNotProcessedException();
		}

		public void Update()
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Post,
				Program.BaseAPIUrl + "/api/Roba/Update"
			);
			APIRequestFailedLog failedLog = null;
			request.Content = System.Net.Http.Json.JsonContent.Create<Roba>(this);
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				return;
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else
				throw new APIResponseNotProcessedException();
		}

		public static Roba Get(int id)
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Get,
				Program.BaseAPIUrl + "/api/Roba/Get?id=" + id
			);
			APIRequestFailedLog failedLog = null;
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				return JsonConvert.DeserializeObject<Roba>(
					response.Content.ReadAsStringAsync().Result
				);
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else
				throw new APIResponseNotProcessedException();
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
			APIRequestFailedLog failedLog = null;
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				return JsonConvert.DeserializeObject<List<Roba>>(
					response.Content.ReadAsStringAsync().Result
				);
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else
				throw new APIResponseNotProcessedException();
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
