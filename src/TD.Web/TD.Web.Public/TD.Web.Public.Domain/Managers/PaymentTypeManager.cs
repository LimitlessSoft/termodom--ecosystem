using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Dtos.PaymentTypes;
using TD.Web.Public.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Domain.Managers
{
    public class PaymentTypeManager : LSCoreBaseManager<PaymentTypeManager, PaymentTypeEntity>, IPaymentTypeManager
    {
        public PaymentTypeManager(ILogger<PaymentTypeManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<PaymentTypeGetDto> GetMultiple()
        {
            var response = new LSCoreListResponse<PaymentTypeGetDto>();

            var qPaymentTypesResponse = Queryable(x => x.IsActive);
            response.Merge(qPaymentTypesResponse);
            if (response.NotOk)
                return response;

            response.Payload = qPaymentTypesResponse.Payload!.ToDtoList<PaymentTypeGetDto, PaymentTypeEntity>();

            return response;
        }
    }
}
