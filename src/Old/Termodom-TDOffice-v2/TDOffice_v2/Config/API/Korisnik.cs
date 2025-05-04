using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TDOffice_v2.API
{
	public partial class Korisnik
	{
		public int ID { get; set; }
		public string Ime { get; set; }
		public string PW { get; set; }
		public int Tip { get; set; }
		public string Nadimak { get; set; }
		public int Status { get; set; }
		public string Mobilni { get; set; }
		public string Komentar { get; set; }
		public string Mail { get; set; }
		public int Poseta { get; set; }
		public string AdresaStanovanja { get; set; }
		public string Opstina { get; set; }
		public int MagacinID { get; set; }
		public DateTime DatumRodjenja { get; set; }
		public bool PrimaObavestenja { get; set; }
		public int? PPID { get; set; }
		public int? Referent { get; set; }
		public DateTime DatumKreiranja { get; set; }
		public bool PoslatRodjendanskiSMS { get; set; }
		public DateTime? DatumOdobrenja { get; set; }
		public DateTime? PoslednjiPutVidjen
		{
			get
			{
				return null;

				List<APIRequestTimeoutExceptionArguments> failedRequestArgs = null;

				for (int i = 0; i < Program.ForbiddenRequestMaxTries; i++)
				{
					HttpRequestMessage request = new HttpRequestMessage(
						HttpMethod.Get,
						Program.BaseAPIUrl + "/Webshop/Korisnik/Poseta/Get?id=" + this.ID
					);
					request.Headers.Authorization =
						new System.Net.Http.Headers.AuthenticationHeaderValue(
							"bearer",
							Program.APIToken
						);

					HttpClient client = new HttpClient();
					HttpResponseMessage response = client.SendAsync(request).Result;

					if (response.StatusCode == HttpStatusCode.OK)
						return Convert.ToDateTime(response.Content.ReadAsStringAsync().Result);
					else if (response.StatusCode == HttpStatusCode.NoContent)
						return null;
					else if (response.StatusCode == HttpStatusCode.InternalServerError)
						throw new APIRequestInternalServerErrorException();

					if (failedRequestArgs == null)
						failedRequestArgs = new List<APIRequestTimeoutExceptionArguments>();

					failedRequestArgs.Add(
						new APIRequestTimeoutExceptionArguments()
						{
							Request = request,
							Response = response,
							Time = DateTime.Now
						}
					);
				}

				throw new APIRequestTImeoutException(failedRequestArgs);
			}
		}
		public string PIB { get; set; }

		public Korisnik() { }

		public static Korisnik Get(int id)
		{
			List<APIRequestTimeoutExceptionArguments> failedRequestArguments = null;
			for (int i = 0; i < Program.ForbiddenRequestMaxTries; i++)
			{
				HttpRequestMessage request = new HttpRequestMessage(
					HttpMethod.Get,
					Program.BaseAPIUrl + "/Webshop/Korisnik/Get?id=" + id
				);
				request.Headers.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue(
						"bearer",
						Program.APIToken
					);

				HttpClient client = new HttpClient();
				HttpResponseMessage response = client.SendAsync(request).Result;
				if (response.StatusCode == HttpStatusCode.OK)
					return JsonConvert.DeserializeObject<Korisnik>(
						response.Content.ReadAsStringAsync().Result
					);
				else if (response.StatusCode == HttpStatusCode.InternalServerError)
					throw new APIRequestInternalServerErrorException();

				if (failedRequestArguments == null)
					failedRequestArguments = new List<APIRequestTimeoutExceptionArguments>();

				failedRequestArguments.Add(
					new APIRequestTimeoutExceptionArguments()
					{
						Request = request,
						Response = response,
						Time = DateTime.Now
					}
				);
			}
			throw new APIRequestTImeoutException(failedRequestArguments);
		}

		public static Task<Korisnik> GetAsync(int id)
		{
			return Task.Run(() =>
			{
				return Get(id);
			});
		}

		public static Korisnik Get(string ime)
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Get,
				Program.BaseAPIUrl + "/Webshop/Korisnik/Get?ime=" + ime
			);
			request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"bearer",
				Program.APIToken
			);

			HttpClient client = new HttpClient();
			List<APIRequestTimeoutExceptionArguments> failedRequestArguments = null;
			for (int i = 0; i < Program.ForbiddenRequestMaxTries; i++)
			{
				HttpResponseMessage response = client.SendAsync(request).Result;
				if (response.StatusCode == HttpStatusCode.OK)
					return response != null
						? JsonConvert.DeserializeObject<Korisnik>(
							response.Content.ReadAsStringAsync().Result
						)
						: null;
				else if (response.StatusCode == HttpStatusCode.InternalServerError)
					throw new APIRequestInternalServerErrorException();

				if (failedRequestArguments == null)
					failedRequestArguments = new List<APIRequestTimeoutExceptionArguments>();

				failedRequestArguments.Add(
					new APIRequestTimeoutExceptionArguments()
					{
						Request = request,
						Response = response,
						Time = DateTime.Now
					}
				);
			}
			throw new APIRequestTImeoutException(failedRequestArguments);
		}

		public static Task<Korisnik> GetAsync(string ime)
		{
			return Task.Run(() =>
			{
				return Get(ime);
			});
		}

		public static List<Korisnik> List()
		{
			if (string.IsNullOrWhiteSpace(Program.APIToken))
				throw new APINullBearerTokenException();

			List<APIRequestTimeoutExceptionArguments> failedRequestArguments = null;
			for (int i = 0; i < Program.ForbiddenRequestMaxTries; i++)
			{
				HttpRequestMessage request = new HttpRequestMessage(
					HttpMethod.Get,
					Program.BaseAPIUrl + "/WebShop/Korisnik/List"
				);
				request.Headers.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue(
						"bearer",
						Program.APIToken
					);

				HttpClient client = new HttpClient();
				HttpResponseMessage response = client.SendAsync(request).Result;
				if (response.StatusCode == HttpStatusCode.OK)
					return JsonConvert.DeserializeObject<List<Korisnik>>(
						response.Content.ReadAsStringAsync().Result
					);
				if (response.StatusCode == HttpStatusCode.InternalServerError)
					throw new APIRequestInternalServerErrorException();

				if (failedRequestArguments == null)
					failedRequestArguments = new List<APIRequestTimeoutExceptionArguments>();

				failedRequestArguments.Add(
					new APIRequestTimeoutExceptionArguments()
					{
						Request = request,
						Response = response,
						Time = DateTime.Now
					}
				);
			}

			throw new APIRequestTImeoutException(failedRequestArguments);
		}

		public static Task<List<Korisnik>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}
	}
}
