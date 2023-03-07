using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
    public static class MagacinManager
    {
        public async static Task<MagacinDictionary> DictionaryAsync(int? godinaBaze = null)
        {
            var response = await TDBrain_v3.GetAsync($"/komercijalno/magacin/dictionary?godinaBaze={godinaBaze ?? DateTime.Now.Year}");

            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<MagacinDictionary>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
