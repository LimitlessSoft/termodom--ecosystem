using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDOffice_v2.TDOffice
{
	public class FirmaManager
	{
		public static async Task<Firma> GetAsync(int id)
		{
			var response = await TDBrain_v3.GetAsync("/tdoffice/firma/get?id=" + id);

			switch ((int)response.StatusCode)
			{
				case 200:
					return JsonConvert.DeserializeObject<Firma>(
						await response.Content.ReadAsStringAsync()
					);
				case 204:
					return null;
				case 500:
					throw new Termodom.Data.Exceptions.APIServerException();
				default:
					throw new Termodom.Data.Exceptions.APIUnhandledStatusException(
						response.StatusCode
					);
			}
		}

		public static async Task<FirmaDictionary> DictionaryAsync()
		{
			var response = await TDBrain_v3.GetAsync("/tdoffice/firma/dictionary");

			switch ((int)response.StatusCode)
			{
				case 200:
					return JsonConvert.DeserializeObject<FirmaDictionary>(
						await response.Content.ReadAsStringAsync()
					);
				case 500:
					throw new Termodom.Data.Exceptions.APIServerException();
				default:
					throw new Termodom.Data.Exceptions.APIUnhandledStatusException(
						response.StatusCode
					);
			}
		}
	}
}
