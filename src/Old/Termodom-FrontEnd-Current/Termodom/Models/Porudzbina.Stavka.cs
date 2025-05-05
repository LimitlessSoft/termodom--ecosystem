using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Termodom.API;

namespace Termodom.Models
{
	public partial class Porudzbina
	{
		public class Stavka
		{
			public int ItemID { get; set; }
			public int PorudzbinaID { get; set; }
			public int RobaID { get; set; }
			public double Kolicina { get; set; }
			public double VPCena { get; set; }
			public double Rabat { get; set; }

			/// <summary>
			/// Azurira kolone Kolicina i VPCena za item
			/// </summary>
			public bool Update()
			{
				DTO.Webshop.StavkaUpdateDTO stavkaUpdateDTO = new DTO.Webshop.StavkaUpdateDTO(
					ItemID,
					Kolicina,
					VPCena,
					Rabat
				);

				HttpRequestMessage request = new HttpRequestMessage(
					HttpMethod.Post,
					Program.BaseAPIUrl + "/Webshop/Stavka/Update"
				);
				request.Headers.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue(
						"bearer",
						Program.APIToken
					);
				request.Content =
					System.Net.Http.Json.JsonContent.Create<DTO.Webshop.StavkaUpdateDTO>(
						stavkaUpdateDTO
					);
				HttpClient client = new HttpClient();
				HttpResponseMessage response = client.Send(request);

				return response.StatusCode == System.Net.HttpStatusCode.OK;
			}

			/// Uklanja stavku iz baze
			/// </summary>
			public static bool Remove(int itemID)
			{
				HttpRequestMessage request = new HttpRequestMessage(
					HttpMethod.Post,
					Program.BaseAPIUrl + "/Webshop/Stavka/Delete?id=" + itemID
				);
				request.Headers.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue(
						"bearer",
						Program.APIToken
					);
				HttpClient client = new HttpClient();
				HttpResponseMessage response = client.Send(request);

				return response.StatusCode == System.Net.HttpStatusCode.OK;
			}

			/// <summary>
			///
			/// </summary>
			/// <exception cref="APIRequestTImeoutException">Javlja se kada nije uspesno uspostavljena veza sa APIjem duze vreme</exception>
			/// <exception cref="APIRequestInternalServerErrorException">Javlja se kada API vrati neobradjenu gresku 500</exception>
			/// <exception cref="APIBadRequestException">Javlja kada ima gresku kod parametara</exception>
			/// <param name="stavka"></param>
			/// <returns></returns>
			public static int Insert(
				int porudzbinaID,
				int robaID,
				double kolicina,
				double vpCena,
				double rabat
			)
			{
				DTO.Webshop.StavkaInsertDTO stavkaInsertDTO = new DTO.Webshop.StavkaInsertDTO(
					porudzbinaID,
					robaID,
					kolicina,
					vpCena,
					rabat
				);

				if (porudzbinaID <= 0)
					throw new APIBadRequestException($"Neispravna porudzbinaID: {porudzbinaID}");

				if (robaID <= 0)
					throw new APIBadRequestException($"Neispravan robaID: {robaID}");

				if (kolicina <= 0)
					throw new APIBadRequestException(
						$"Neispravna kolicina: {kolicina.ToString("#,##0.###")}"
					);

				if (vpCena <= 0)
					throw new APIBadRequestException(
						$"Neispravna cena: {vpCena.ToString("#,##0.00")}"
					);

				//if (rabat < 0)
				//    throw new APIBadRequestException($"Neispravan rabat: {rabat.ToString("#,##0.00")}");

				HttpRequestMessage request = new HttpRequestMessage(
					HttpMethod.Post,
					Program.BaseAPIUrl + "/Webshop/Stavka/Insert"
				);
				request.Content =
					System.Net.Http.Json.JsonContent.Create<DTO.Webshop.StavkaInsertDTO>(
						stavkaInsertDTO
					);

				APIRequestFailedLog failedLog = null;
				HttpResponseMessage response = APIRequest.Send(request, out failedLog);

				if (response.StatusCode == System.Net.HttpStatusCode.Created)
					return Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
				else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
					throw new APIRequestTimeoutException(failedLog);
				else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					throw new APIRequestInternalServerErrorException();
				else
					throw new APIBadRequestException(response.Content.ReadAsStringAsync().Result);
			}

			public static Stavka Get(int stavkaID)
			{
				HttpRequestMessage request = new HttpRequestMessage(
					HttpMethod.Get,
					Program.BaseAPIUrl + "/Webshop/Stavka/Get?id=" + stavkaID
				);
				APIRequestFailedLog failedLog = null;
				HttpResponseMessage response = APIRequest.Send(request, out failedLog);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
					return Newtonsoft.Json.JsonConvert.DeserializeObject<Porudzbina.Stavka>(
						response.Content.ReadAsStringAsync().Result
					);
				else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
					throw new APIRequestTimeoutException(failedLog);
				else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					throw new APIRequestInternalServerErrorException();
				else
					throw new APIBadRequestException(response.Content.ReadAsStringAsync().Result);
			}

			/// <summary>
			/// Vraca listu stavki za porudzbinu
			/// </summary>
			/// <param name="PorudzbinaID"></param>
			/// <returns></returns>
			public static List<Stavka> List(int PorudzbinaID)
			{
				HttpRequestMessage request = new HttpRequestMessage(
					HttpMethod.Get,
					Program.BaseAPIUrl + "/Webshop/Stavka/List?porudzbinaID=" + PorudzbinaID
				);
				APIRequestFailedLog failedLog = null;
				HttpResponseMessage response = APIRequest.Send(request, out failedLog);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
					return Porudzbina.Stavka.ConvertToList(
						Newtonsoft.Json.JsonConvert.DeserializeObject<List<DTO.Webshop.StavkaDTO>>(
							response.Content.ReadAsStringAsync().Result
						)
					);
				else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
					throw new APIRequestTimeoutException(failedLog);
				else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					throw new APIRequestInternalServerErrorException();
				else
					throw new APIBadRequestException(response.Content.ReadAsStringAsync().Result);
			}

			public static List<Stavka> List()
			{
				HttpRequestMessage request = new HttpRequestMessage(
					HttpMethod.Get,
					Program.BaseAPIUrl + "/Webshop/Stavka/List"
				);
				APIRequestFailedLog failedLog = null;
				HttpResponseMessage response = APIRequest.Send(request, out failedLog);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
					return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Stavka>>(
						response.Content.ReadAsStringAsync().Result
					);
				else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
					throw new APIRequestTimeoutException(failedLog);
				else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					throw new APIRequestInternalServerErrorException();
				else
					throw new APIBadRequestException(response.Content.ReadAsStringAsync().Result);
			}

			public static Task<List<Stavka>> ListAsync()
			{
				return Task.Run(() =>
				{
					return List();
				});
			}

			public static List<Stavka> ConvertToList(List<DTO.Webshop.StavkaDTO> list)
			{
				List<Stavka> newList = new List<Stavka>();
				foreach (DTO.Webshop.StavkaDTO p in list)
					newList.Add(
						new Stavka()
						{
							ItemID = p.ID,
							Kolicina = p.Kolicina,
							VPCena = p.VPCena,
							Rabat = p.Rabat,
							RobaID = p.RobaID,
							PorudzbinaID = p.PorudzbinaID
						}
					);

				return newList;
			}
		}
	}
}
