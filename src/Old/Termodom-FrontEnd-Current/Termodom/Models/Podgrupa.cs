using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Termodom.API;

namespace Termodom.Models
{
	// TODO: Transferovati u API
	public class Podgrupa
	{
		public int ID { get; set; }
		public int GrupaID { get; set; }
		public string Naziv { get; set; }
		public string Slika { get; set; }

		public Podgrupa() { }

		/// <summary>
		/// Vraca podgrupu iz baze sa datim ID-om.
		/// Ukoliko je ne pronadje vraca null
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Podgrupa Get(int id)
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Get,
				Program.BaseAPIUrl + "/Webshop/Podgrupa/Get?id=" + id
			);

			APIRequestFailedLog failedLog = null;
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				return JsonConvert.DeserializeObject<Podgrupa>(
					response.Content.ReadAsStringAsync().Result
				);
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
				return null;
			else
				throw new APIResponseNotProcessedException();
		}

		public static Podgrupa Get(string naziv)
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Get,
				Program.BaseAPIUrl + "/Webshop/Podgrupa/Get?naziv=" + naziv
			);

			APIRequestFailedLog failedLog = null;
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				return JsonConvert.DeserializeObject<Podgrupa>(
					response.Content.ReadAsStringAsync().Result
				);
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
				return null;
			else
				throw new APIResponseNotProcessedException();
		}

		public static List<Podgrupa> List()
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Get,
				Program.BaseAPIUrl + "/Webshop/Podgrupa/List"
			);

			APIRequestFailedLog failedLog = null;
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				return JsonConvert.DeserializeObject<List<Podgrupa>>(
					response.Content.ReadAsStringAsync().Result
				);
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
				return null;
			else
				throw new APIResponseNotProcessedException();
		}

		public static Task<List<Podgrupa>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}
	}
}
