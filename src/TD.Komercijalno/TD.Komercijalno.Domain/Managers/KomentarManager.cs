using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;

namespace TD.Komercijalno.Domain.Managers
{
    public class KomentarManager (
        ILogger<KomentarManager> logger, 
        KomercijalnoDbContext dbContext,
        IKomentarRepository komentarRepository
        )
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

        public void FlushComments(FlushCommentsRequest request) =>
            komentarRepository.Flush(request.VrDok, request.BrDok);

        public KomentarDto Get(GetKomentarRequest request)
        {
            request.Validate();

            var komentar = komentarRepository.Get(request.BrDok,request.VrDok);

            return komentar.ToKomentarDto();
        }

        public KomentarDto Update(UpdateKomentarRequest request)
        {
            var komentar = komentarRepository.Get(request.BrDok, request.VrDok);

            komentar.InjectFrom(request);
            komentar.JavniKomentar = request.Komentar;

            dbContext.SaveChanges();

            return komentar.ToKomentarDto();
        }
    }
}
