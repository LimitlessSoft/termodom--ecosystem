using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDWeb
{

    public class Proizvod
    {
        public int RobaID { get; set; }
        public string Naziv { get; set; }
        public string KatBr { get; set; }
        public string JM { get; set; }
        public string Slika { get; set; }
        public int PodgrupaID { get; set; }
        public Int16 Aktivan { get; set; }
        public double PDV { get; set; }
        public int DisplayIndex { get; set; }
        public string KratakOpis { get; set; }
        public int Poseta { get; set; }
        public Int16 Rasprodaja { get; set; }
        public string Keywords { get; set; }
        public double NabavnaCena { get; set; }
        public double ProdajnaCena { get; set; }
        public string Rel { get; set; }
        public string DetaljanOpis { get; set; }
        public Int16 Klasifikacija { get; set; }
        public double TransportnoPakovanje { get; set; }
        public string TransportnoPakovanjeJM { get; set; }
        public Int16 KupovinaSamoUTransportnomPakovanju { get; set; }
        public string IstaknutiProizvodi { get; set; }
        public Int16 UpozorenjeZalihaMalihStovarista { get; set; }
        public Int16 CenovnaGrupaID { get; set; }
        public string PovezaniProizvodi { get; set; }
        public double AktuelniRabat { get; set; }
        public List<int> Podgrupe { get; set; }
        public int Parent { get; set; }
        public int BrendID { get; set; }

        public void Update()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BaseAPIUrl + "/Webshop/Proizvod/Update");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Program.APIToken);
            request.Content = System.Net.Http.Json.JsonContent.Create<Proizvod>(this);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.SendAsync(request).Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Error updating proizvod!");
        }

        public static Proizvod Get(int id)
        {
            Task<DTO.Webshop.ProizvodGetDTO> wp = Task.Run(() =>
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/Webshop/Proizvod/Get?id=" + id);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Program.APIToken);

                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.SendAsync(request).Result;
                return JsonConvert.DeserializeObject<DTO.Webshop.ProizvodGetDTO>(response.Content.ReadAsStringAsync().Result);
            });

            Task<API.Roba> r = API.Roba.GetAsync(id);

            return Proizvod.Join(wp.Result, r.Result);
        }
        public static Task<Proizvod> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                return Get(id);
            });
        }
        public static Proizvod Get(string rel)
        {
            Task<DTO.Webshop.ProizvodGetDTO> wp = Task.Run(() =>
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/Webshop/Proizvod/Get?rel=" + rel);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Program.APIToken);

                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.SendAsync(request).Result;
                return JsonConvert.DeserializeObject<DTO.Webshop.ProizvodGetDTO>(response.Content.ReadAsStringAsync().Result);
            });

            Task<API.Roba> r = API.Roba.GetAsync(wp.Result.RobaID);

            return Proizvod.Join(wp.Result, r.Result);
        }
        public static Task<Proizvod> GetAsync(string rel)
        {
            return Task.Run(() =>
            {
                return Get(rel);
            });
        }

        public static List<Proizvod> List()
        {
            Task<List<DTO.Webshop.ProizvodGetDTO>> wp = Task.Run(() =>
            {
                int status = -1;
                HttpResponseMessage response = null;
                while (status != 200)
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/Webshop/Proizvod/List");
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Program.APIToken);

                    HttpClient client = new HttpClient();
                    response = client.SendAsync(request).Result;

                    status = (int)response.StatusCode;
                }

                string responseString = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<List<DTO.Webshop.ProizvodGetDTO>>(responseString);
            });

            Task<List<API.Roba>> r = API.Roba.ListAsync();

            return Proizvod.Join(wp.Result, r.Result);
        }
        public static Task<List<Proizvod>> ListAsync()
        {
            return Task.Run(() =>
            {
                return List();
            });
        }

        public static Proizvod Join(DTO.Webshop.ProizvodGetDTO proizvodGetDTO, API.Roba roba)
        {
            return new Proizvod()
            {
                RobaID = proizvodGetDTO.RobaID,
                Naziv = roba.Naziv,
                KatBr = roba.KatBr,
                JM = roba.JM,
                Slika = proizvodGetDTO.Slika,
                PodgrupaID = proizvodGetDTO.PodgrupaID,
                Aktivan = proizvodGetDTO.Aktivan,
                PDV = proizvodGetDTO.PDV,
                DisplayIndex = proizvodGetDTO.DisplayIndex,
                KratakOpis = proizvodGetDTO.KratakOpis,
                Poseta = proizvodGetDTO.Poseta,
                Rasprodaja = proizvodGetDTO.Rasprodaja,
                Keywords = proizvodGetDTO.Keywords,
                NabavnaCena = proizvodGetDTO.NabavnaCena,
                ProdajnaCena = proizvodGetDTO.ProdajnaCena,
                Rel = proizvodGetDTO.Rel,
                DetaljanOpis = proizvodGetDTO.DetaljanOpis,
                Klasifikacija = proizvodGetDTO.Klasifikacija,
                TransportnoPakovanje = proizvodGetDTO.TransportnoPakovanje,
                TransportnoPakovanjeJM = proizvodGetDTO.TransportnoPakovanjeJM,
                KupovinaSamoUTransportnomPakovanju = proizvodGetDTO.KupovinaSamoUTransportnomPakovanju,
                IstaknutiProizvodi = proizvodGetDTO.IstaknutiProizvodi,
                UpozorenjeZalihaMalihStovarista = proizvodGetDTO.UpozorenjeZalihaMalihStovarista,
                CenovnaGrupaID = proizvodGetDTO.CenovnaGrupaID,
                PovezaniProizvodi = proizvodGetDTO.PovezaniProizvodi,
                AktuelniRabat = proizvodGetDTO.AktuelniRabat,
                Podgrupe = proizvodGetDTO.Podgrupe,
                Parent = proizvodGetDTO.Parent,
                BrendID = proizvodGetDTO.BrendID
            };
        }
        public static List<Proizvod> Join(List<DTO.Webshop.ProizvodGetDTO> proizvodGetDTO, List<API.Roba> roba)
        {
            List<Proizvod> list = new List<Proizvod>();
            foreach (var p in proizvodGetDTO)
            {
                API.Roba r = roba.Where(x => x.ID == p.RobaID).FirstOrDefault();
                if (r == null)
                    //r = new API.Roba() { JM = "UNDEFINED", KatBr = "UNDEFINED", Naziv = "UNDEFINED" };
                    continue;

                list.Add(new Proizvod()
                {
                    RobaID = p.RobaID,
                    Naziv = r.Naziv,
                    KatBr = r.KatBr,
                    JM = r.JM,
                    Slika = p.Slika,
                    PodgrupaID = p.PodgrupaID,
                    Aktivan = p.Aktivan,
                    PDV = p.PDV,
                    DisplayIndex = p.DisplayIndex,
                    KratakOpis = p.KratakOpis,
                    Poseta = p.Poseta,
                    Rasprodaja = p.Rasprodaja,
                    Keywords = p.Keywords,
                    NabavnaCena = p.NabavnaCena,
                    ProdajnaCena = p.ProdajnaCena,
                    Rel = p.Rel,
                    DetaljanOpis = p.DetaljanOpis,
                    Klasifikacija = p.Klasifikacija,
                    TransportnoPakovanje = p.TransportnoPakovanje,
                    TransportnoPakovanjeJM = p.TransportnoPakovanjeJM,
                    KupovinaSamoUTransportnomPakovanju = p.KupovinaSamoUTransportnomPakovanju,
                    IstaknutiProizvodi = p.IstaknutiProizvodi,
                    UpozorenjeZalihaMalihStovarista = p.UpozorenjeZalihaMalihStovarista,
                    CenovnaGrupaID = p.CenovnaGrupaID,
                    PovezaniProizvodi = p.PovezaniProizvodi,
                    AktuelniRabat = p.AktuelniRabat,
                    Podgrupe = p.Podgrupe,
                    Parent = p.Parent,
                    BrendID = p.BrendID
                });
            }

            return list;
        }
    }
}
