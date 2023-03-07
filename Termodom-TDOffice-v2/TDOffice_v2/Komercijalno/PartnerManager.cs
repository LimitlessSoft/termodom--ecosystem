using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
    public static class PartnerManager
    {
        /// <summary>
        /// Vraca dictionary partnera iz baze
        /// </summary>
        /// <param name="godinaBaze">Godina baze iz koje ce se izvlaciti partneri. Ukoliko se ne prosledi ili se prosledi null, uzimace u obzir trenutnu godinu.</param>
        /// <returns></returns>
        /// <exception cref="Termodom.Data.Exceptions.APIServerException"></exception>
        /// <exception cref="Termodom.Data.Exceptions.APIUnhandledStatusException"></exception>
        public static async Task<PartnerDictionary> DictionaryAsync(int? godinaBaze)
        {
            var response = await TDBrain_v3.GetAsync($"/komercijalno/partner/dictionary?godinaBaze={godinaBaze ?? DateTime.Now.Year}");

            var r = await response.Content.ReadAsStringAsync();
            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<PartnerDictionary>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
