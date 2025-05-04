using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Termodom.API;

namespace Termodom.Models
{
	public class Zanimanje // TODO: ZanimanjeModel????
	{
		public int ID { get; set; }
		public string Naziv { get; set; }

		public static List<Zanimanje> List()
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Get,
				Program.BaseAPIUrl + "/Webshop/Zanimanje/List"
			);

			APIRequestFailedLog failedLog = null;
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				return JsonConvert.DeserializeObject<List<Zanimanje>>(
					response.Content.ReadAsStringAsync().Result
				);
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else
				throw new APIResponseNotProcessedException();
		}

		public static Task<List<Zanimanje>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}

		public static void Insert(string naziv)
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Post,
				Program.BaseAPIUrl + "/Webshop/Zanimanje/Insert?naziv=" + naziv
			);

			APIRequestFailedLog failedLog = null;
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.Created)
				return;
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				throw new APIBadRequestException(response.Content.ReadAsStringAsync().Result);
			else
				throw new APIResponseNotProcessedException();
		}

		public static void Remove(int id)
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Post,
				Program.BaseAPIUrl + "/Webshop/Porudzbina/Remove?id=" + id
			);
			APIRequestFailedLog failedLog = null;
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
	}
}
