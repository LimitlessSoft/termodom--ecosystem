using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
	public static class BankaManager
	{
		public static async Task<BankaDictionary> DictionaryAsync()
		{
			return await TDBrain_v3
				.GetAsync($"/komercijalno/banka/dictionary")
				.HandleTDBrainResponse<BankaDictionary>();
		}
	}
}
