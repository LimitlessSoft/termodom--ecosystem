using TD.Web.Common.Contracts.Dtos.PaymentTypes;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Http;

namespace TD.Web.Common.Contracts.Interfaces.IManagers
{
    public interface IPaymentTypeManager : ILSCoreBaseManager
    {
        LSCoreListResponse<PaymentTypesGetDto> GetMultiple();
    }
}