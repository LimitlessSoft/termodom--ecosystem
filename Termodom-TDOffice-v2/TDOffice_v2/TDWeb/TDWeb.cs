using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDWeb
{
    public class TDWeb
    {
        public static string connectionString { get; set; } = "Server=174.138.184.42;Database=termodom_db_main;Uid=masdos_mdoas;Pwd=j1cnH38$;Pooling=false;SSL Mode=None";

        public static void AzurirajIronCene()
        {
            List<Komercijalno.RobaUMagacinu> rum = Komercijalno.RobaUMagacinu.ListByMagacinID(50);
            List<Proizvod> proizvodi = Proizvod.List();
                
            Parallel.ForEach(proizvodi, p =>
            {
                Komercijalno.RobaUMagacinu r = rum.Where(x => x.RobaID == p.RobaID).FirstOrDefault();

                if (r == null)
                    return;

                p.ProdajnaCena = r.ProdajnaCena;
                p.Update();
            });
        }

        public static bool SetApiKey(string username, string password)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BaseAPIUrl + "/api/Korisnik/GetToken?username=" + username + "&password=" + password);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.SendAsync(request).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Program.APIToken = response.Content.ReadAsStringAsync().Result;
                return true;
            }

            return false;
        }
    }
}
