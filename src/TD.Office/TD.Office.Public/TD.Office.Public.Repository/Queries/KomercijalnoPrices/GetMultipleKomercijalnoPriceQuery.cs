using LSCore.Repository;
using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Http.Interfaces;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Repository.Queries.KomercijalnoPrices
{
    public class GetMultipleKomercijalnoPriceQuery : LSCoreBaseQuery<IQueryable<KomercijalnoPriceEntity>>
    {
        public override ILSCoreResponse<IQueryable<KomercijalnoPriceEntity>> Execute(ILSCoreDbContext dbContext)
        {
            return new LSCoreResponse<IQueryable<KomercijalnoPriceEntity>>(dbContext.AsQueryable<KomercijalnoPriceEntity>());
        }
    }
}
