using LSCore.Mapper.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.PaymentTypes;

namespace TD.Web.Public.Contracts.DtoMappings.PaymentTypes;

public class PaymentTypeGetDtoMapping : ILSCoreMapper<PaymentTypeEntity, PaymentTypeGetDto>
{
	public PaymentTypeGetDto ToMapped(PaymentTypeEntity sender) =>
		new() { Id = sender.Id, Name = sender.Name };
}
