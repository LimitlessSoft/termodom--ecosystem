using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class KomentarManager : LSCoreBaseManager<KomentarManager>, IKomentarManager
    {
        public KomentarManager(ILogger<KomentarManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {

        }

        public LSCoreResponse<KomentarDto> Create(CreateKomentarRequest request)
        {
            var response = new LSCoreResponse<KomentarDto>();

            if (request.IsRequestInvalid(response))
                return response;

            var komentar = new Komentar();
            komentar.InjectFrom(request);
            komentar.JavniKomentar = request.Komentar;

            Insert<Komentar>(komentar);

            response.Payload = komentar.ToKomentarDto();
            return response;
        }

        public LSCoreResponse<KomentarDto> Get(GetKomentarRequest request)
        {
            var response = new LSCoreResponse<KomentarDto>();

            if (request.IsRequestInvalid(response))
                return response;

            var komentarResponse = First<Komentar>(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok);

            if (komentarResponse.Status == System.Net.HttpStatusCode.NotFound)
                return LSCoreResponse<KomentarDto>.NotFound();

            response.Merge(komentarResponse);
            if (response.NotOk)
                return response;

            response.Payload = komentarResponse.Payload.ToKomentarDto();
            return response;
        }
    }
}
