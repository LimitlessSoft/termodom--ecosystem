using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Termodom.Data.Entities.DBSettings;

namespace TDOffice_v2.Komercijalno
{
    public static class BazaManager
    {
        /// <summary>
        /// Vraca ID-eve jedinstvenih puatanja baza
        /// </summary>
        /// <returns></returns>
        public static async Task<List<DistinctConnectionInfo>> DistinctConnectionInfoListAsync()
        {
            var list = await ListAsync();

            List<DistinctConnectionInfo> output = new List<DistinctConnectionInfo>();
            foreach(var connInfo in list)
            {
                if (output.Any(x => x.PutanjaDoBaze == connInfo.PutanjaDoBaze))
                    continue;

                output.Add(new DistinctConnectionInfo()
                {
                    Godina = connInfo.Godina,
                    PutanjaDoBaze = connInfo.PutanjaDoBaze
                });
            }

            return output;
        }
        /// <summary>
        /// Vraca listu konekcija do baza
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Termodom.Data.Exceptions.APIServerException"></exception>
        /// <exception cref="Termodom.Data.Exceptions.APIUnhandledStatusException"></exception>
        public static async Task<List<ConnectionInfo>> ListAsync()
        {
            var response = await TDBrain_v3.GetAsync("/dbsettings/baza/komercijalno/list");

            switch((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<List<ConnectionInfo>>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }

        }
    }
}
