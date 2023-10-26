using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Termodom.API;

namespace Termodom.DTO.Webshop
{
    public class KorisnikDTO
    {
        public int ID { get; set; }
        public int Zanimanje { get; set; }

        public static KorisnikDTO Get(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/Webshop/Korisnik/Get?id=" + id);
            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<DTO.Webshop.KorisnikDTO>(response.Content.ReadAsStringAsync().Result);
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                throw new APIResponseNoContentException();
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            else throw new APIResponseNotProcessedException();
        }

        public static void Insert(int id, int zanimanje, int aktivan = 1)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BaseAPIUrl + $"/Webshop/Korisnik/Insert?id={id}&zanimanje={zanimanje}&aktivan={aktivan}");
            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);


            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return;
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                throw new APIResponseNoContentException();
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new APIRequestEndPointNotFoundException();
            else throw new APIResponseNotProcessedException();
        }


    }
}
