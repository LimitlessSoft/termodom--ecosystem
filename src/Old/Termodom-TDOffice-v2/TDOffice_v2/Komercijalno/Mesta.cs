using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TDOffice_v2.Komercijalno
{
	public static class Mesta
	{
		/// <summary>
		/// Vraca read only dictionary objekata mesta
		/// </summary>
		/// <param name="godinaBaze">Ukoliko se prosledi null vratice dictionary iz baze trenutne godine</param>
		/// <returns></returns>
		/// <exception cref="Termodom.Data.Exceptions.APIServerException"></exception>
		/// <exception cref="Termodom.Data.Exceptions.APIUnhandledStatusException"></exception>
		/// <exception cref="Exception"></exception>
		public static async Task<Termodom.Data.Entities.Komercijalno.MestoDictionary> DictionaryAsync(
			int? godinaBaze = null
		)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();

			if (godinaBaze != null)
				parameters.Add("godinaBaze", godinaBaze.ToString());

			var response = await TDBrain_v3.GetAsync("/komercijalno/mesto/dictionary", parameters);

			switch ((int)response.StatusCode)
			{
				case 200:
					return JsonConvert.DeserializeObject<Termodom.Data.Entities.Komercijalno.MestoDictionary>(
						await response.Content.ReadAsStringAsync()
					);
				case 500:
					throw new Termodom.Data.Exceptions.APIServerException();
				default:
					throw new Termodom.Data.Exceptions.APIUnhandledStatusException(
						response.StatusCode
					);
			}

			throw new Exception("asd");
		}
	}
}
