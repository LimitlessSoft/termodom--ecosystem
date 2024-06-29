using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using TD.Komercijalno.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Omu.ValueInjecter;

namespace TD.Komercijalno.Domain.Managers
{
    public class KomentarManager (ILogger<KomentarManager> logger, KomercijalnoDbContext dbContext)
        : LSCoreManagerBase<KomentarManager>(logger, dbContext), IKomentarManager
    {
        public KomentarDto Create(CreateKomentarRequest request)
        {
            request.Validate();

            var komentar = new Komentar();
            komentar.InjectFrom(request);
            komentar.JavniKomentar = request.Komentar;

            InsertNonLSCoreEntity(komentar);

            return komentar.ToKomentarDto();
        }

        public KomentarDto Get(GetKomentarRequest request)
        {
            request.Validate();

            var komentar = Queryable<Komentar>().FirstOrDefault(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok);

            if(komentar == null)
                throw new LSCoreNotFoundException();

            return komentar.ToKomentarDto();
        }
    }
}
