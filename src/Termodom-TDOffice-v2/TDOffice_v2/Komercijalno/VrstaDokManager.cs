using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
    public class VrstaDokManager
    {
        public static async Task<VrstaDok> GetAsync(int godina, int vrDok)
        {
            var response = await TDBrain_v3.GetAsync($"/Komercijalno/VrstaDok/Get?vrDok={vrDok}&godina={godina}");
            switch((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<VrstaDok>(await response.Content.ReadAsStringAsync());
                case 204:
                    return null;
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }

        public static async Task<VrstaDokDictionary> DictionaryAsync(int? godina = null)
        {
            var response = await TDBrain_v3.GetAsync($"/komercijalno/vrstadok/dictionary?godinaBaze={godina}");

            switch((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<VrstaDokDictionary>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
