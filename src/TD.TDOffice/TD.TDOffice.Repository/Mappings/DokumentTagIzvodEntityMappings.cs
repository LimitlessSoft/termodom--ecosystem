using Omu.ValueInjecter;
using TD.Core.Contracts;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.TDOffice.Repository.Mappings
{
    public partial class DokumentTagIzvodEntityMappings : IMap<DokumentTagIzvod, DokumentTagizvodPutRequest>
    {
        public void Map(DokumentTagIzvod entity, DokumentTagizvodPutRequest request)
        {
            entity.InjectFrom(request);

            if(request.Id.HasValue)
                entity.Id = request.Id.Value;

            if (request.BrojDokumentaIzvoda.HasValue)
                entity.BrojDokumentaIzvoda = request.BrojDokumentaIzvoda.Value;
        }
    }
}
