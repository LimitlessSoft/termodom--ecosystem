using LSCore.Repository;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Http.Interfaces;
using TD.Office.Common.Contracts.Entities;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using LSCore.Contracts.Http;

namespace TD.Office.Common.Repository.Commands.KomercijalnoPrices
{
    public class UpdateKomercijalnoPricesCommand : LSCoreBaseCommand<List<RobaUMagacinuGetDto>>
    {
        public override ILSCoreResponse Execute(ILSCoreDbContext dbContext)
        {
            var komercijalnoPrices = dbContext.AsQueryable<KomercijalnoPriceEntity>().AsEnumerable();

            dbContext.Delete(komercijalnoPrices);

            var list = new List<KomercijalnoPriceEntity>();
            Request!.ForEach(roba =>
            {
                list.Add(new KomercijalnoPriceEntity()
                {
                    RobaId = roba.RobaId,
                    NabavnaCenaBezPDV = (decimal)roba.NabavnaCena,
                    ProdajnaCenaBezPDV = (decimal)roba.ProdajnaCena
                });
            });

            dbContext.InsertMultiple(list);

            return new LSCoreResponse();
        }
    }
}
