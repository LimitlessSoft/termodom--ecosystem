using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TDOffice_v2.Komercijalno
{
    public static class DokumentManager
    {
        /// <summary>
        /// Vraca listu svih dokumenata is svih baza za izabranu godinu
        /// </summary>
        /// <param name="godinaBaze"></param>
        /// <returns></returns>
        /// <exception cref="Termodom.Data.Exceptions.APIServerException"></exception>
        /// <exception cref="Termodom.Data.Exceptions.APIUnhandledStatusException"></exception>
        public static async Task<Termodom.Data.Entities.Komercijalno.DokumentDictionary> DictionaryAsync(int idBaze,
            int? godinaBaze = null,
            int[] vrDok = null,
            int[] magacinId = null,
            DateTime? odDatuma = null,
            DateTime? doDatuma = null)
        {
            string epString = $"/komercijalno/dokument/dictionary?idBaze={idBaze}&godinaBaze={godinaBaze ?? DateTime.Now.Year}";
            if (vrDok != null && vrDok.Length > 0)
                epString += $"&vrdok={string.Join("&vrdok=", vrDok)}";

            if (magacinId != null && magacinId.Length > 0)
                epString += $"&magacinid={string.Join("&magacinId=", magacinId)}";

            if (odDatuma != null)
                epString += $"&odDatuma={((DateTime)odDatuma).ToString("MM-dd-yyyy")}";

            if (doDatuma != null)
                epString += $"&doDatuma={((DateTime)doDatuma).ToString("MM-dd-yyyy")}";

            var response = await TDBrain_v3.GetAsync(epString);

            if ((int)response.StatusCode == 200)
                return new Termodom.Data.Entities.Komercijalno.DokumentDictionary(JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, Termodom.Data.Entities.Komercijalno.Dokument>>>(await response.Content.ReadAsStringAsync()));
            else if ((int)response.StatusCode == 500)
                throw new Termodom.Data.Exceptions.APIServerException();
            else
                throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
        }
    }
}
