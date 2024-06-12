using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Dtos.PaymentTypes;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;

namespace TD.Web.Public.Domain.Managers;

public class PaymentTypeManager (ILogger<PaymentTypeManager> logger, WebDbContext dbContext)
    : LSCoreManagerBase<PaymentTypeManager, PaymentTypeEntity>(logger, dbContext), IPaymentTypeManager
{
    public List<PaymentTypeGetDto> GetMultiple() =>
        Queryable()
            .Where(x => x.IsActive)
            .ToDtoList<PaymentTypeEntity, PaymentTypeGetDto>();
}