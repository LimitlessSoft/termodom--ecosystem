using TD.Web.Common.Contracts.Dtos.PaymentTypes;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface IPaymentTypeManager
{
    List<PaymentTypesGetDto> GetMultiple();
}