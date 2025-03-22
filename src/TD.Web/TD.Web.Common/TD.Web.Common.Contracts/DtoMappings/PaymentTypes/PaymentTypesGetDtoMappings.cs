using LSCore.Mapper.Contracts;
using TD.Web.Common.Contracts.Dtos.PaymentTypes;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.PaymentTypes;

public class PaymentTypesGetDtoMappings : ILSCoreMapper<PaymentTypeEntity, PaymentTypesGetDto>
{
	public PaymentTypesGetDto ToMapped(PaymentTypeEntity sender) =>
		new PaymentTypesGetDto { Id = sender.Id, Name = sender.Name };
}
