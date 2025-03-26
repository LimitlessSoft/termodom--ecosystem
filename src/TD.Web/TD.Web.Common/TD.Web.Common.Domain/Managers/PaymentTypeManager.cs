using LSCore.Mapper.Domain;
using TD.Web.Common.Contracts.Dtos.PaymentTypes;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Domain.Managers;

public class PaymentTypeManager(IPaymentTypeRepository repository) : IPaymentTypeManager
{
	public List<PaymentTypesGetDto> GetMultiple() =>
		repository.GetMultiple().ToMappedList<PaymentTypeEntity, PaymentTypesGetDto>();
}
