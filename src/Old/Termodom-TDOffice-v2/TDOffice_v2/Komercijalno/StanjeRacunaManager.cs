using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
	public static class StanjeRacunaManager
	{
		public static async Task<StanjeRacuna> GetAsync(
			int bazaId,
			int godinaBaze,
			string tekuciRacun
		)
		{
			var response = await TDBrain_v3.GetAsync(
				$"/komercijalno/stanjeracuna/get?bazaId={bazaId}&godinaBaze={godinaBaze}&tekuciRacun={tekuciRacun}"
			);

			switch ((int)response.StatusCode)
			{
				case 200:
					return JsonConvert.DeserializeObject<StanjeRacuna>(
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

		public static async Task<StanjeRacunaDictionary> DictionaryAsync(
			int bazaId,
			int? godinaBaze = null
		)
		{
			var response = await TDBrain_v3.GetAsync(
				$"/komercijalno/stanjeracuna/dictionary?bazaId={bazaId}&godinaBaze={godinaBaze ?? DateTime.Now.Year}"
			);

			switch ((int)response.StatusCode)
			{
				case 200:
					return JsonConvert.DeserializeObject<StanjeRacunaDictionary>(
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
