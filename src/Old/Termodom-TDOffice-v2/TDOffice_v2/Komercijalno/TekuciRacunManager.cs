using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDOffice_v2.Komercijalno
{
	public static class TekuciRacunManager
	{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
		public class TekuciRacunInsertRequestBody
		{
			[Required]
			public int PPID { get; set; }

			[Required]
			public string Racun { get; set; } = "";

			[Required]
			public int? BankaID { get; set; }
			public string Valuta { get; set; } = "RSD";
			public double Stanje { get; set; } = 0;
			public int? MagacinID { get; set; }
		}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

		public static async Task<bool> InsertAsync(
			int ppid,
			string racun,
			int bankaID,
			string valuta,
			double stanje,
			int? magacinID
		)
		{
			var response = await TDBrain_v3.PostAsync(
				$"/komercijalno/tekuciracun/insert",
				new TekuciRacunInsertRequestBody()
				{
					BankaID = bankaID,
					PPID = ppid,
					Racun = racun,
					Valuta = valuta,
					Stanje = stanje,
					MagacinID = magacinID
				}
			);

			switch ((int)response.StatusCode)
			{
				case 201:
					return true;
				case 500:
					throw new Termodom.Data.Exceptions.APIServerException();
				case 400:
					throw new Exception(await response.Content.ReadAsStringAsync());
				default:
					throw new Termodom.Data.Exceptions.APIUnhandledStatusException(
						response.StatusCode
					);
			}
		}
	}
}
