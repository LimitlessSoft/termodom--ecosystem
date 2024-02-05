using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Public.Contracts.Dtos.PaymentTypes;

namespace TD.Web.Public.Contracts.Interfaces.IManagers
{
    public interface IPaymentTypeManager : ILSCoreBaseManager
    {
        LSCoreListResponse<PaymentTypeGetDto> GetMultiple();
    }
}
