using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Dtos.PaymentTypes;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using LSCore.Domain.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;

namespace TD.Web.Common.Domain.Managers;

public class PaymentTypeManager (ILogger<PaymentTypeManager> logger, WebDbContext dbContext)
    : LSCoreManagerBase<PaymentTypeManager, PaymentTypeEntity>(logger, dbContext), IPaymentTypeManager
{
    public List<PaymentTypesGetDto> GetMultiple() =>
        Queryable()
            .Where(x => x.IsActive)
            .ToDtoList<PaymentTypeEntity, PaymentTypesGetDto>();
}