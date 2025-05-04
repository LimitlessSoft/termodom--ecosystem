using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Termodom.API;
using Termodom.Models;

namespace Termodom.Models
{
	public partial class Porudzbina
	{
		#region Stavke
		/// <summary>
		/// Stavke porudzbine
		/// Ne ucitavam stavke iz porudzbine dokle god nema potrebe za tim.
		/// Ukoliko se jednom zatraze stavke (Pourdzbina.Stavke), te informacije ce biti
		/// skladistene u varijablu _stavke kao buffer tako da sledeci put ne moraju da se ucitavaju
		/// iz baze
		/// </summary>
		private List<Stavka> _stavke { get; set; } = new List<Stavka>();

		[JsonIgnore]
		public List<Stavka> Stavke
		{
			get
			{
				if (_stavke.Count == 0)
					_stavke = Stavka.List(ID);

				return _stavke;
			}
		}
		#endregion
		#region Parametri
		public int BrDokKom { get; set; }
		public int UserID { get; set; }
		public int ID { get; set; }
		public int MagacinID { get; set; }
		public int? PPID { get; set; }
		public string InterniKomentar { get; set; }
		public string Hash { get; set; }
		public int? ReferentObrade { get; set; }
		public DateTime Datum { get; set; }
		public PorudzbinaStatus Status { get; set; }
		public PorudzbinaNacinUplate NacinUplate { get; set; }
		public double K { get; set; }
		public double UstedaKorisnik { get; set; }
		public double UstedaKlijent { get; set; }
		public bool Dostava { get; set; }
		public string KomercijalnoInterniKomentar { get; set; }
		public string KomercijalnoKomentar { get; set; }
		public string KomercijalnoNapomena { get; set; }
		public string Napomena { get; set; }
		public string AdresaIsporuke { get; set; }
		public string KontaktMobilni { get; set; }
		public string ImeIPrezime { get; set; }
		#endregion

		public Porudzbina() { }

		/// <summary>
		/// Azurira podatke porudzbine na API-ju
		/// </summary>
		/// <exception cref="APIRequestTimeoutException">Veza sa API-jem nije uspostavljena</exception>
		/// <exception cref="APIRequestInternalServerErrorException">Nepoznata greska na strani API servera</exception>
		public void Update()
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Post,
				Program.BaseAPIUrl + "/Webshop/Porudzbina/Update"
			);
			request.Content = System.Net.Http.Json.JsonContent.Create<DTO.Webshop.PorudzbinaDTO>(
				DTO.Webshop.PorudzbinaDTO.ConvertPorudzbina(this)
			); // TODO: DTO.WebShop.PorudzbinaUpdateDTO

			APIRequestFailedLog failedLog = null;
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				return;
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
		}

		public bool JeProfi()
		{
			if (UserID > 0)
				return true;

			return false;
		}

		/// <summary>
		/// Vraca veleprodajnu vrednost porudzbine
		/// </summary>
		/// <returns></returns>
		public double VPVrednost()
		{
			return Stavke.Sum(x => x.Kolicina * x.VPCena);
		}

		/// <summary>
		/// Vraca maloprodajnu vrednost porudzbine
		/// </summary>
		/// <returns></returns>
		public double MPVrednost()
		{
			List<Proizvod> proizvodi = Proizvod.List();

			return Stavke.Sum(x =>
				x.Kolicina
				* x.VPCena
				* ((100 + proizvodi.Where(y => y.RobaID == x.RobaID).FirstOrDefault().PDV) / 100)
			);
		}

		/// <summary>
		/// Vraca maloprodajnu vrednost porudzbine ali ne ucitava proizvode iz baze vec koristi prosledjeni buffer
		/// </summary>
		/// <param name="proizvodiBuffer"></param>
		/// <returns></returns>
		public double MPVrednost(List<Proizvod> proizvodiBuffer)
		{
			try
			{
				double ukupno = Stavke.Sum(x =>
					x.Kolicina
					* x.VPCena
					* (
						(
							100
							+ proizvodiBuffer.Where(y => y.RobaID == x.RobaID).FirstOrDefault().PDV
						) / 100
					)
				);
				return ukupno;
			}
			catch
			{
				return -1;
			}
		}

		/// <summary>
		/// Ucitava stavke porudzbine iz zadate liste stavki
		/// </summary>
		/// <param name="stavke"></param>
		public void UcitajStavke(List<Stavka> stavke)
		{
			_stavke = stavke.Where(x => x.PorudzbinaID == ID).ToList();
		}

		/// <summary>
		/// Kreira porudzbinu i vraca hash
		/// </summary>
		/// <param name="porudzbina"></param>
		/// <returns></returns>
		public static string Insert(Porudzbina porudzbina) // TODO: PorudzbinaInsertDTO
		{
			// TODO: Insert(parametar1, parametar2, paramtera3...)
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Post,
				Program.BaseAPIUrl + "/Webshop/Porudzbina/Insert"
			);
			// TODO: DTO.WebShop.PorudzbinaInsertDTO
			APIRequestFailedLog failedLog = null;
			request.Content = System.Net.Http.Json.JsonContent.Create<DTO.Webshop.PorudzbinaDTO>(
				DTO.Webshop.PorudzbinaDTO.ConvertPorudzbina(porudzbina)
			);
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.Created)
				return response.Content.ReadAsStringAsync().Result;
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				throw new APIBadRequestException();
			else
				throw new APIResponseNotProcessedException();
		}

		public static Porudzbina GetHash(string hash) // TODO: Get(...) > Spakovati u funkciju dole
		{
			// TODO: API Request
			List<Porudzbina> list = Porudzbina.List();
			return list.Where(t => t.Hash == hash).FirstOrDefault(); // TODO: .FirstOrDefault(Expression);
		}

		public static Porudzbina Get(int id)
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Get,
				Program.BaseAPIUrl + "/Webshop/Porudzbina/Get?id=" + id
			);

			APIRequestFailedLog failedLog = null;
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				return Porudzbina.ConvertPorudzbinaDTO(
					JsonConvert.DeserializeObject<DTO.Webshop.PorudzbinaDTO>(
						response.Content.ReadAsStringAsync().Result
					)
				);
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else
				throw new APIResponseNotProcessedException();
		}

		/// <summary>
		/// Vraca listu svih porudzbina
		/// </summary>
		/// <returns></returns>
		public static List<Porudzbina> List()
		{
			HttpRequestMessage request = new HttpRequestMessage(
				HttpMethod.Get,
				Program.BaseAPIUrl + "/Webshop/Porudzbina/List"
			);

			APIRequestFailedLog failedLog = null;
			HttpResponseMessage response = APIRequest.Send(request, out failedLog);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				return Porudzbina.ConvertToListPorudzbinaDTO(
					JsonConvert.DeserializeObject<List<DTO.Webshop.PorudzbinaDTO>>(
						response.Content.ReadAsStringAsync().Result
					)
				);
			else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
				throw new APIRequestTimeoutException(failedLog);
			else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				throw new APIRequestInternalServerErrorException();
			else
				throw new APIResponseNotProcessedException();
		}

		/// <summary>
		/// Vraca listu svih porudzbina
		/// </summary>
		/// <returns></returns>
		public static List<Porudzbina> ListByUserID(int userID) // TODO: List(...) > Spakovati u jednu funkciji
		{
			// TODO: API Request
			return Porudzbina.List().Where(t => t.UserID == userID).ToList();
		}

		public static Task<Porudzbina> GetAsync(int id)
		{
			return Task.Run(() =>
			{
				return Get(id);
			});
		}

		/// <summary>
		/// Vraca listu svih porudzbina
		/// </summary>
		/// <returns></returns>
		public static async Task<List<Porudzbina>> ListAsync()
		{
			return await Task.Run<List<Porudzbina>>(() =>
			{
				return Porudzbina.List();
			});
		}

		public static Porudzbina ConvertPorudzbinaDTO(DTO.Webshop.PorudzbinaDTO p) // TODO: Remove
		{
			// TODO: Remove
			return new Porudzbina()
			{
				ID = p.ID,
				BrDokKom = p.BrDokKomercijalno,
				MagacinID = p.MagacinID,
				UserID = p.KorisnikID,
				UstedaKlijent = p.UstedaKlijent,
				UstedaKorisnik = p.UstedaKorisnik,
				Datum = p.Datum,
				KomercijalnoInterniKomentar = p.KomercijalnoInterniKomentar,
				KomercijalnoNapomena = p.KomercijalnoNapomena,
				KomercijalnoKomentar = p.KomercijalnoKomentar,
				KontaktMobilni = p.KontaktMobilni,
				InterniKomentar = p.InterniKomentar,
				AdresaIsporuke = p.AdresaIsporuke,
				ImeIPrezime = p.ImeIPrezime,
				Hash = p.Hash,
				K = p.K,
				Dostava = Convert.ToBoolean(p.Dostava),
				PPID = p.PPID,
				NacinUplate = (PorudzbinaNacinUplate)p.NacinPlacanja,
				Status = (PorudzbinaStatus)p.Status,
				Napomena = p.Napomena,
				ReferentObrade = p.ReferentObrade
			};
		}

		public static List<Porudzbina> ConvertToListPorudzbinaDTO(
			List<DTO.Webshop.PorudzbinaDTO> list
		) // TODO: Remove
		{
			// TODO: Remove
			List<Porudzbina> newList = new List<Porudzbina>();
			foreach (DTO.Webshop.PorudzbinaDTO p in list)
				newList.Add(
					new Porudzbina()
					{
						ID = p.ID,
						BrDokKom = p.BrDokKomercijalno,
						UserID = p.KorisnikID,
						Datum = p.Datum,
						Status = (PorudzbinaStatus)p.Status,
						MagacinID = p.MagacinID,
						PPID = p.PPID,
						InterniKomentar = p.InterniKomentar,
						ReferentObrade = p.ReferentObrade,
						NacinUplate = (PorudzbinaNacinUplate)p.NacinPlacanja,
						Hash = p.Hash,
						K = p.K,
						UstedaKlijent = p.UstedaKlijent,
						UstedaKorisnik = p.UstedaKorisnik,
						Dostava = p.Dostava == 1 ? true : false,
						KomercijalnoInterniKomentar = p.KomercijalnoInterniKomentar,
						KomercijalnoNapomena = p.KomercijalnoNapomena,
						KomercijalnoKomentar = p.KomercijalnoKomentar,
						Napomena = p.Napomena,
						AdresaIsporuke = p.AdresaIsporuke,
						KontaktMobilni = p.KontaktMobilni,
						ImeIPrezime = p.ImeIPrezime
					}
				);

			return newList;
		}
	}
}
