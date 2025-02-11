using LSCore.Domain.Extensions;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Public.Contracts.Dtos.PaymentTypes;
using TD.Web.Public.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Domain.Managers;

public class PaymentTypeManager (IPaymentTypeRepository repository)
    : IPaymentTypeManager
{
    public List<PaymentTypeGetDto> GetMultiple() =>
        repository.GetMultiple().ToDtoList<PaymentTypeEntity, PaymentTypeGetDto>();
}