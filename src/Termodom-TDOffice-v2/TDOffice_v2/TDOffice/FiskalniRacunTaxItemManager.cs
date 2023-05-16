using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDOffice_v2.TDOffice
{
    public static class FiskalniRacunTaxItemManager
    {
        public async static Task InsertAsync(List<FiskalniRacunTaxItem> taxItems)
        {
            var response = await TDBrain_v3.PostAsync($"/TDOffice/FiskalniRacunTaxItem/Insert", taxItems);

            switch ((int)response.StatusCode)
            {
                case 201:
                    return;
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                case 400:
                    throw new Exception(await response.Content.ReadAsStringAsync());
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
        public async static Task<FiskalniRacunTaxItemDictionary> DictionaryAsync(string invoiceNumber)
        {
            var response = await TDBrain_v3.GetAsync($"/TDOffice/FiskalniRacunTaxItem/Dictionary?invoiceNumber=" + invoiceNumber);

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
