using Newtonsoft.Json;
using System.Threading.Tasks;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
    public static class RobaUMagacinuManager
    {
        public static async Task<RobaUMagacinuDictionary> Dictionary(int[] magacinId = null)
        {
            var response = await TDBrain_v3.GetAsync($"/Komercijalno/RobaUMagacinu/Dictionary?{(magacinId == null || magacinId.Length == 0 ? "" : "magacinId=" + string.Join("&magacinId=", magacinId))}");

            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<RobaUMagacinuDictionary>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
