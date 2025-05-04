using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TDOffice_v2.Komercijalno
{
	public static class DokumentManager
	{
		public class DokumentInsertRequestBody
		{
			public int BazaId { get; set; }
			public int? GodinaBaze { get; set; }
			public int VrDok { get; set; }
			public int MagacinId { get; set; }
			public string? InterniBroj { get; set; }
			public int? PPID { get; set; }
			public int NuId { get; set; }
			public int? KomercijalnoKorisnikId { get; set; }
			public bool DozvoliDaljeIzmeneUKomercijalnom { get; set; } = true;
		}

		public static async Task<Termodom.Data.Entities.Komercijalno.Dokument> GetAsync(
			int bazaId,
			int vrDok,
			int brDok,
			int? godinaBaze = null
		)
		{
			string epString =
				$"/komercijalno/dokument/get?bazaID={bazaId}&godina={godinaBaze ?? DateTime.Now.Year}&vrDok={vrDok}&brDok={brDok}";

			var response = await TDBrain_v3.GetAsync(epString);

			if ((int)response.StatusCode == 200)
				return JsonConvert.DeserializeObject<Termodom.Data.Entities.Komercijalno.Dokument>(
					await response.Content.ReadAsStringAsync()
				);
			if ((int)response.StatusCode == 204)
				return null;
			else if ((int)response.StatusCode == 500)
				throw new Termodom.Data.Exceptions.APIServerException();
			else
				throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
		}

		/// <summary>
		/// Vraca listu svih dokumenata is svih baza za izabranu godinu
		/// </summary>
		/// <param name="godinaBaze"></param>
		/// <returns></returns>
		/// <exception cref="Termodom.Data.Exceptions.APIServerException"></exception>
		/// <exception cref="Termodom.Data.Exceptions.APIUnhandledStatusException"></exception>
		public static async Task<Termodom.Data.Entities.Komercijalno.DokumentDictionary> DictionaryAsync(
			int idBaze,
			int? godinaBaze = null,
			int[] vrDok = null,
			int[] magacinId = null,
			DateTime? odDatuma = null,
			DateTime? doDatuma = null
		)
		{
			string epString =
				$"/komercijalno/dokument/dictionary?idBaze={idBaze}&godinaBaze={godinaBaze ?? DateTime.Now.Year}";
			if (vrDok != null && vrDok.Length > 0)
				epString += $"&vrdok={string.Join("&vrdok=", vrDok)}";

			if (magacinId != null && magacinId.Length > 0)
				epString += $"&magacinid={string.Join("&magacinId=", magacinId)}";

			if (odDatuma != null)
				epString += $"&odDatuma={((DateTime)odDatuma).ToString("MM-dd-yyyy")}";

			if (doDatuma != null)
				epString += $"&doDatuma={((DateTime)doDatuma).ToString("MM-dd-yyyy")}";

			var response = await TDBrain_v3.GetAsync(epString);

			if ((int)response.StatusCode == 200)
				return new Termodom.Data.Entities.Komercijalno.DokumentDictionary(
					JsonConvert.DeserializeObject<
						Dictionary<
							int,
							Dictionary<int, Termodom.Data.Entities.Komercijalno.Dokument>
						>
					>(await response.Content.ReadAsStringAsync())
				);
			else if ((int)response.StatusCode == 500)
				throw new Termodom.Data.Exceptions.APIServerException();
			else
				throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
		}

		public static async Task PresaberiAsync(int bazaId, int vrDok, int brDok, int? godinaBaze)
		{
			string epString =
				$"/komercijalno/dokument/presaberi?bazaId={bazaId}&godinaBaze={godinaBaze ?? DateTime.Now.Year}&vrDok={vrDok}&brDok={brDok}";

			var response = await TDBrain_v3.GetAsync(epString);

			if ((int)response.StatusCode == 200)
				return;
			else if ((int)response.StatusCode == 500)
				throw new Termodom.Data.Exceptions.APIServerException();
			else
				throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
		}

		public static async Task<int> InsertAsync(DokumentInsertRequestBody request)
		{
			string epString = $"/komercijalno/dokument/insert";

			var response = await TDBrain_v3.PostAsync(epString, request);

			if ((int)response.StatusCode == 201)
				return JsonConvert.DeserializeObject<int>(
					await response.Content.ReadAsStringAsync()
				);
			else if ((int)response.StatusCode == 500)
				throw new Termodom.Data.Exceptions.APIServerException();
			else
				throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
		}
	}
}
