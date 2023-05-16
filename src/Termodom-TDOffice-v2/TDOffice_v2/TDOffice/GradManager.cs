using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDOffice_v2.TDOffice
{
    public static class GradManager
    {
        public static async Task<GradDictionary> DictionaryAsync()
        {
            var response = await TDBrain_v3.GetAsync($"/tdoffice_v2/grad/dictionary");

            var r = await response.Content.ReadAsStringAsync();
            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<GradDictionary>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
