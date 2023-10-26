using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Termodom.Data.Entities.Komercijalno;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TDOffice_v2.Komercijalno
{
    public static class IzvodManager
    {
        public static async Task<IzvodDictionary> DictionaryAsync(int bazaId, int? godinaBaze = null, string pozNaBroj = null)
        {
            var response = await TDBrain_v3.GetAsync($"/komercijalno/izvod/dictionary?bazaId={bazaId}&godinaBaze={godinaBaze ?? DateTime.Now.Year}&pozNaBroj={pozNaBroj}");

            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<IzvodDictionary>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
        public static async Task<double> DugujeSumAsync(int bazaId, int godinaBaze, string tekuciRacun)
        {
            var response = await TDBrain_v3.GetAsync($"/komercijalno/izvod/duguje/sum?bazaId={bazaId}&godinaBaze={godinaBaze}&tekuciracun={tekuciRacun}");

            var r = await response.Content.ReadAsStringAsync();
            switch ((int)response.StatusCode)
            {
                case 200:
                    return Convert.ToDouble(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
        public static async Task<double> PotrazujeSumAsync(int bazaId, int godinaBaze, string tekuciRacun)
        {
            var response = await TDBrain_v3.GetAsync($"/komercijalno/izvod/potrazuje/sum?bazaId={bazaId}&godinaBaze={godinaBaze}&tekuciracun={tekuciRacun}");

            switch ((int)response.StatusCode)
            {
                case 200:
                    return Convert.ToDouble(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }

}
