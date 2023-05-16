using Newtonsoft.Json;
using System.Threading.Tasks;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
    public static class TarifeManager
    {
        /// <summary>
        /// Vraca dictionary tarifa iz baze.
        /// </summary>
        /// <param name="godinaBaze">Ukoliko se prosledi null vratice podatke iz baze trenutne godine</param>
        /// <returns></returns>
        public static async Task<TarifaDictionary> DictionaryAsync(int? godinaBaze = null)
        {
            return await TDBrain_v3.GetAsync($"/komercijalno/tarifa/dictionary?godinaBaze={godinaBaze}").HandleTDBrainResponse<TarifaDictionary>();
        }
    }
}
