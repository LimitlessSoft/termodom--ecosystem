using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
    public static class StavkaManager
    {
        public class StavkaInsertRequestBody
        {
            public int BazaId { get; set; }
            public int? GodinaBaze { get; set; }
            public int VrDok { get; set; }
            public int BrDok { get; set; }
            public int RobaId { get; set; }
            public double Kolicina { get; set; }
            public double? ProdajnaCenaBezPdv { get; set; }
            public double Rabat { get; set; }
        }
        public class NapraviUsluguRequestBody
        {
            public int BazaId { get; set; }
            public int? GodinaBaze { get; set; }
            public int VrDok { get; set; }
            public int BrDok { get; set; }
            public int RobaId { get; set; }
            public double CenaBezPdv { get; set; }
            public double Kolicina { get; set; }
            public double Rabat { get; set; } = 0;
        }

        public static async Task<int> InsertAsync(StavkaInsertRequestBody request)
        {
            string epString = $"/komercijalno/stavka/insert";

            var response = await TDBrain_v3.PostAsync(epString, request);

            if ((int)response.StatusCode == 201)
                return JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            else if ((int)response.StatusCode == 500)
                throw new Termodom.Data.Exceptions.APIServerException();
            else
                throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
        }

        public static async Task<int> NapraviUsluguAsync(NapraviUsluguRequestBody request)
        {
            string epString = $"/komercijalno/stavka/napraviuslugu";

            var response = await TDBrain_v3.PostAsync(epString, request);

            if ((int)response.StatusCode == 201)
                return JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            else if ((int)response.StatusCode == 500)
                throw new Termodom.Data.Exceptions.APIServerException();
            else
                throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
        }

        public static async Task<StavkaDictionary> DictionaryAsync(int bazaId, int vrDok, int brDok, int? godinaBaze)
        {
            string epString = $"/komercijalno/stavka/dictionary?bazaid={bazaId}&vrdok={vrDok}&brdok={brDok}&godina={godinaBaze ?? DateTime.Now.Year}";

            var response = await TDBrain_v3.GetAsync(epString);

            if ((int)response.StatusCode == 200)
                return JsonConvert.DeserializeObject<StavkaDictionary>(await response.Content.ReadAsStringAsync());
            else if ((int)response.StatusCode == 500)
                throw new Termodom.Data.Exceptions.APIServerException();
            else
                throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
        }
    }
}
