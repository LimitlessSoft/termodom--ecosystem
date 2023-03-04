using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
    public static class BankaManager
    {
        public static async Task<BankaDictionary> DictionaryAsync()
        {
            var response = await TDBrain_v3.GetAsync($"/komercijalno/banka/dictionary");

            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<BankaDictionary>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
