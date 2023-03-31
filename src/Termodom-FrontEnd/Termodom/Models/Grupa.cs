using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Termodom.Models
{
    // TODO: Transfer u API
    public class Grupa
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public int DisplayIndex { get; set; }

        public Grupa()
        {

        }

        /// <summary>
        /// Vraca grupu iz baze po ID-u. Ukoliko je ne nadje vraca null.
        /// </summary>
        public static Grupa Get(int id)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Program.APIToken);

            string baseUrl = Program.BaseAPIUrl + "/Webshop/Grupa/Get?id=" + id;

            var response = client.GetAsync(baseUrl).Result;

            return JsonConvert.DeserializeObject<Grupa>(response.Content.ReadAsStringAsync().Result);
        }
        /// <summary>
        /// Vraca grupu iz baze po nazivuu. Ukoliko je ne nadje vraca null.
        /// </summary>
        public static Grupa Get(string naziv)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Program.APIToken);

            string baseUrl = Program.BaseAPIUrl + "/Webshop/Grupa/Get?naziv=" + naziv;

            var response = client.GetAsync(baseUrl).Result;

            return JsonConvert.DeserializeObject<Grupa>(response.Content.ReadAsStringAsync().Result);
        }
        /// <summary>
        /// Vraca listu svih grupa
        /// </summary>
        /// <returns></returns>
        public static List<Grupa> List()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Program.APIToken);

            string baseUrl = Program.BaseAPIUrl + "/Webshop/Grupa/List";

            HttpResponseMessage response = null;

            while((response = client.GetAsync(baseUrl).Result).StatusCode != System.Net.HttpStatusCode.OK) { }

            string responseText = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<Grupa>>(responseText);
        }
        /// <summary>
        /// Vraca listu svih grupa
        /// </summary>
        /// <returns></returns>
        public static Task<List<Grupa>> ListAsync()
        {
            return Task.Run(() =>
            {
                return List();
            });
        }
    }
}
