using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDOffice_v2.TDOffice
{
    public static class FiskalniRacunTaxItemManager
    {
        public async static Task<FiskalniRacunTaxItemDictionary> DictionaryAsync()
        {
            var response = await TDBrain_v3.GetAsync($"/TDOffice/FiskalniRacunTaxItem/Dictionary");

            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<FiskalniRacunTaxItemDictionary>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                case 400:
                    throw new Exception(await response.Content.ReadAsStringAsync());

                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
