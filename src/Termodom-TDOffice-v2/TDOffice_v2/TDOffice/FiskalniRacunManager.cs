using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDOffice_v2.TDOffice
{
    public static class FiskalniRacunManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="odDatuma">Format datuma dd-MM-yyyy</param>
        /// <param name="doDatuma">Format datuma dd-MM-yyyy</param>
        /// <returns></returns>
        /// <exception cref="Termodom.Data.Exceptions.APIServerException"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Termodom.Data.Exceptions.APIUnhandledStatusException"></exception>
        public async static Task<FiskalniRacunDictionary> DictionaryAsync(string odDatuma, string doDatuma)
        {
            var response = await TDBrain_v3.GetAsync($"/tdoffice/fiskalniracun/dictionary?odDatuma={odDatuma}&doDatuma={doDatuma}");

            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<FiskalniRacunDictionary>(await response.Content.ReadAsStringAsync());
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
