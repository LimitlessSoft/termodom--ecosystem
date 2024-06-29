using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using TD.Komercijalno.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Omu.ValueInjecter;

namespace TD.Komercijalno.Domain.Managers
{
    public class DokumentManager (ILogger<DokumentManager> logger, KomercijalnoDbContext komercijalnoDbContext)
        : LSCoreManagerBase<DokumentManager>(logger, komercijalnoDbContext), IDokumentManager
    {
        public DokumentDto Create(DokumentCreateRequest request)
        {
            request.Validate();

            var poslednjiBrDok = 0;

            var posledjiBrDokZaVrstuZaMagacin = Queryable<VrstaDokMag>()
                .FirstOrDefault(x => x.VrDok == request.VrDok &&
                                     x.MagacinId == request.MagacinId);
            if (posledjiBrDokZaVrstuZaMagacin == null)
            {
                var vrstaDokResponse = Queryable<VrstaDok>().FirstOrDefault(x => x.Id == request.VrDok);
                if (vrstaDokResponse == null)
                    throw new LSCoreNotFoundException();

                poslednjiBrDok = vrstaDokResponse.Poslednji ?? 0;
            }
            
            var dokument = new Dokument();
            dokument.InjectFrom(request);
            dokument.BrDok = poslednjiBrDok + 1;
            dokument.Kurs = 1;

            if (dokument.Linked == null)
                dokument.Linked = NextLinked(new DokumentNextLinkedRequest()
                {
                    MagacinId = dokument.MagacinId,
                    Datum = DateTime.Now
                });

            if (dokument.MtId == null)
            {
                var magacin = Queryable<Magacin>().FirstOrDefault(x => x.Id == request.MagacinId);
                if (magacin == null)
                    throw new LSCoreNotFoundException();

                dokument.MtId = magacin.MtId;
            }

            InsertNonLSCoreEntity(dokument);
            return dokument.ToDokumentDto();
        }

        public DokumentDto Get(DokumentGetRequest request)
        {
            var dokument = Queryable<Dokument>()
                .Include(x => x.Stavke)
                .FirstOrDefault(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok);
            if (dokument == null)
                throw new LSCoreNotFoundException();

            return dokument.ToDokumentDto();
        }

        public List<DokumentDto> GetMultiple(DokumentGetMultipleRequest request)
        {
            return Queryable<Dokument>()
                .Where(x =>
                    (!request.VrDok.HasValue || x.VrDok == request.VrDok.Value) &&
                    (string.IsNullOrWhiteSpace(request.IntBroj) || x.IntBroj == request.IntBroj) &&
                    (!request.KodDok.HasValue || x.KodDok == request.KodDok.Value) &&
                    (!request.Flag.HasValue || x.Flag == request.Flag.Value) &&
                    (!request.DatumOd.HasValue || x.Datum >= request.DatumOd.Value) &&
                    (!request.DatumDo.HasValue || x.Datum <= request.DatumDo.Value) &&
                    (string.IsNullOrWhiteSpace(request.Linked) || x.Linked == request.Linked) &&
                    (!request.MagacinId.HasValue || x.MagacinId == request.MagacinId.Value) &&
                    (!request.NUID.HasValue || x.NuId == request.NUID) &&
                    (!request.PPID.HasValue || x.PPID == request.PPID.Value))
                .Include(x => x.Stavke)
                .ToList()
                .ToDokumentListDto();
        }

        public string NextLinked(DokumentNextLinkedRequest request)
        {
            var maxLinkedDokument = Queryable<Dokument>()
                .Where(x =>
                    x.MagacinId == request.MagacinId &&
                    (
                        string.IsNullOrWhiteSpace(x.Linked) ||
                        x.Linked != "9999999999"
                    ))
                .OrderBy(x => Convert.ToDouble(x.Linked))
                .FirstOrDefault();

            return maxLinkedDokument == null ? "0000000000" : Convert.ToDouble(maxLinkedDokument.Linked).ToString("0000000000");
        }

        public void SetNacinPlacanja(DokumentSetNacinPlacanjaRequest request)
        {
            var dokument = Queryable<Dokument>().FirstOrDefault(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok);

            if(dokument == null)
                throw new LSCoreNotFoundException();

            dokument.NuId = request.NUID;

            Update(dokument);
        }
    }
}
