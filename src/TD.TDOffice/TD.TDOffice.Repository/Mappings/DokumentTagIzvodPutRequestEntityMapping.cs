using Omu.ValueInjecter;
using TD.Core.Contracts;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.TDOffice.Repository.Mappings
{
    public class DokumentTagIzvodPutRequestEntityMapping : IMap<DokumentTagIzvod, DokumentTagizvodPutRequest>
    {
        public void Map(DokumentTagIzvod originalEntity, DokumentTagizvodPutRequest request)
        {
            originalEntity.InjectFrom(request);

            if(request.Id.HasValue)
                originalEntity.Id = request.Id.Value;

            if (request.BrojDokumentaIzvoda.HasValue && !request.Id.HasValue)
                originalEntity.BrojDokumentaIzvoda = request.BrojDokumentaIzvoda.Value;

            if (request.UnosDuguje.HasValue)
                originalEntity.UnosDuguje = request.UnosDuguje.Value;

            if (request.UnosPocetnoStanje.HasValue)
                originalEntity.UnosPocetnoStanje = request.UnosPocetnoStanje.Value;

            if (request.UnosPotrazuje.HasValue)
                originalEntity.UnosPotrazuje = request.UnosPotrazuje.Value;

            if (request.Korisnik.HasValue)
                originalEntity.Korisnik = request.Korisnik.Value;
        }
    }
}
