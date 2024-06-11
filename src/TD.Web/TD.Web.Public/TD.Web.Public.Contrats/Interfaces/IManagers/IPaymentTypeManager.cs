using TD.Web.Public.Contracts.Dtos.PaymentTypes;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface IPaymentTypeManager
{
    List<PaymentTypeGetDto> GetMultiple();
}