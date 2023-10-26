using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
    public static class PFRSManager
    {
        /// <summary>
        /// Vraca dictionary PFR-a
        /// </summary>
        /// <param name="bazaId"></param>
        /// <param name="godinaBaze"></param>
        /// <returns></returns>
        /// <exception cref="Termodom.Data.Exceptions.APIServerException"></exception>
        /// <exception cref="Termodom.Data.Exceptions.APIUnhandledStatusException"></exception>
        public static async Task<PFRDictionary> DictionaryAsync(int bazaId, int? godinaBaze)
        {
            var response = await TDBrain_v3.GetAsync($"/komercijalno/PFR/dictionary?bazaId={bazaId}&godinaBaze={godinaBaze ?? DateTime.Now.Year}");

            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<PFRDictionary>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
