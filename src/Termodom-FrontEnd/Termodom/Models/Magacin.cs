using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Termodom.API;

namespace Termodom.Models
{
    public class Magacin
    {
        public int ID { get; set; }
        public string Adresa { get; set; }
        public string Grad { get; set; }
        public string Email { get; set; }
        public string Koordinate { get; set; }
        public string Telefon { get; set; }
        public string Naziv { get; set; }
      
        public static void Insert(string adresa, string grad, string email, string koordinate, string telefon, string naziv)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BaseAPIUrl + "/api/Magacin/Insert");
            DTO.Webshop.MagacinDTO magacin = new DTO.Webshop.MagacinDTO()
            {
                Adresa = adresa,
                Grad = grad,
                Email = email,
                Koordinate = koordinate,
                Telefon = telefon,
                Naziv = naziv
                
            };
            request.Content = System.Net.Http.Json.JsonContent.Create<DTO.Webshop.MagacinDTO>(magacin);
           
            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return;
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new APIBadRequestException(response.Content.ReadAsStringAsync().Result);
            else
                throw new APIResponseNotProcessedException();
        }
        public static Magacin Get(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/api/Magacin/Get?id=" + id);

            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Magacin>(response.Content.ReadAsStringAsync().Result);
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return null;
            else
                throw new APIResponseNotProcessedException();
        }

        public static List<Magacin> List()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/api/Magacin/List");

            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<List<Magacin>>(response.Content.ReadAsStringAsync().Result);
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return null;
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else
                throw new APIResponseNotProcessedException();
        }

        public static Task<List<Magacin>> ListAsync()
        {
            return Task.Run(() =>
            {
                return List();
            });
        }
        public void Update()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BaseAPIUrl + "/api/Magacin/Update");
            request.Content = System.Net.Http.Json.JsonContent.Create<Magacin>(this);
            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return;
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new APIBadRequestException(response.Content.ReadAsStringAsync().Result);
            else
                throw new APIResponseNotProcessedException();
        }

    }
}
