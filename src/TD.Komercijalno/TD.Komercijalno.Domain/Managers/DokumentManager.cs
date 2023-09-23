using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class DokumentManager : BaseManager<DokumentManager>, IDokumentManager
    {
        public DokumentManager(ILogger<DokumentManager> logger, KomercijalnoDbContext komercijalnoDbContext)
            : base(logger, komercijalnoDbContext)
        {
        }

        public Response<DokumentDto> Create(DokumentCreateRequest request)
        {
            var response = new Response<DokumentDto>();

            if (request.IsRequestInvalid(response))
                return response;

            int poslednjiBrDok = 0;

            var posledjiBrDokZaVrstuZaMagacinResponse = First<VrstaDokMag>(x =>
                x.VrDok == request.VrDok &&
                x.MagacinId == request.MagacinId);
            if (posledjiBrDokZaVrstuZaMagacinResponse.Status != System.Net.HttpStatusCode.NotFound)
            {
                response.Merge(posledjiBrDokZaVrstuZaMagacinResponse);
                if (response.NotOk)
                    return response;
            }

            if (posledjiBrDokZaVrstuZaMagacinResponse.Status == System.Net.HttpStatusCode.NotFound)
            {
                var vrstaDokResponse = First<VrstaDok>(x => x.Id == request.VrDok);
                response.Merge(vrstaDokResponse);
                if (response.NotOk)
                    return response;

                poslednjiBrDok = vrstaDokResponse.Payload.Poslednji ?? 0;
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
                }).Payload;

            if (dokument.MtId == null)
            {
                var magacinResponse = First<Magacin>(x => x.Id == request.MagacinId);
                response.Merge(magacinResponse);
                if (response.NotOk)
                    return response;

                dokument.MtId = magacinResponse.Payload.MtId;
            }

            Insert<Dokument>(dokument);

            response.Status = System.Net.HttpStatusCode.Created;
            response.Payload = dokument.ToDokumentDto();
            return response;
        }

        public Response<DokumentDto> Get(DokumentGetRequest request)
        {
            var dok = Queryable<Dokument>()
                .Where(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok)
                .Include(x => x.Stavke)
                .FirstOrDefault();
            if (dok == null)
                return Response<DokumentDto>.NotFound();

            return new Response<DokumentDto>(dok.ToDokumentDto());
        }

        public ListResponse<DokumentDto> GetMultiple(DokumentGetMultipleRequest request)
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
                .ToDokumentDtoListResponse();
        }

        public Response<string> NextLinked(DokumentNextLinkedRequest request)
        {
            var response = new Response<string>();
            var maxLinkedDokument = Queryable<Dokument>()
                .Where(x =>
                    x.MagacinId == request.MagacinId &&
                    (
                        string.IsNullOrWhiteSpace(x.Linked) ||
                        x.Linked != "9999999999"
                    ))
                .OrderBy(x => Convert.ToDouble(x.Linked))
                .FirstOrDefault();

            response.Payload = maxLinkedDokument == null ? "0000000000" : Convert.ToDouble(maxLinkedDokument.Linked).ToString("0000000000");
            return response;
        }

        public Response SetNacinPlacanja(DokumentSetNacinPlacanjaRequest request)
        {
            var response = new Response();

            var dokumentResponse = First<Dokument>(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok);

            if (dokumentResponse.Status == System.Net.HttpStatusCode.NotFound)
                return Response.NotFound();

            response.Merge(dokumentResponse);
            if (response.NotOk)
                return response;

            dokumentResponse.Payload.NuId = request.NUID;

            Update(dokumentResponse.Payload);
            return response;
        }
    }
}
