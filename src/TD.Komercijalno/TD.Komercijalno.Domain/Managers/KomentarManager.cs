using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class KomentarManager : BaseManager<KomentarManager>, IKomentarManager
    {
        public KomentarManager(ILogger<KomentarManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {

        }

        public Response<KomentarDto> Create(CreateKomentarRequest request)
        {
            var response = new Response<KomentarDto>();

            if (request.IsRequestInvalid(response))
                return response;

            var komentar = new Komentar();
            komentar.InjectFrom(request);
            komentar.JavniKomentar = request.Komentar;

            Insert<Komentar>(komentar);

            response.Payload = komentar.ToKomentarDto();
            return response;
        }

        public Response<KomentarDto> Get(GetKomentarRequest request)
        {
            var response = new Response<KomentarDto>();

            if (request.IsRequestInvalid(response))
                return response;

            var komentarResponse = First<Komentar>(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok);

            if (komentarResponse.Status == System.Net.HttpStatusCode.NotFound)
                return Response<KomentarDto>.NotFound();

            response.Merge(komentarResponse);
            if (response.NotOk)
                return response;

            response.Payload = komentarResponse.Payload.ToKomentarDto();
            return response;
        }
    }
}
