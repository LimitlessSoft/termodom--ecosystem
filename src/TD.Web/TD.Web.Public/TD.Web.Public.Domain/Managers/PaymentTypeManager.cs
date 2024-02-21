using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Dtos.PaymentTypes;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;
using LSCore.Contracts.Http;

namespace TD.Web.Public.Domain.Managers
{
    public class PaymentTypeManager : LSCoreBaseManager<PaymentTypeManager, PaymentTypeEntity>, IPaymentTypeManager
    {
        public PaymentTypeManager(ILogger<PaymentTypeManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<PaymentTypeGetDto> GetMultiple() =>
            Queryable()
                .LSCoreFilters(x => x.IsActive)
                .ToLSCoreListResponse<PaymentTypeGetDto, PaymentTypeEntity>();
    }
}
