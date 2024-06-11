using TD.Web.Public.Contracts.Dtos.PaymentTypes;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;

namespace TD.Web.Public.Contracts.DtoMappings.PaymentTypes;

public class PaymentTypeGetDtoMapping : ILSCoreDtoMapper<PaymentTypeEntity, PaymentTypeGetDto>
{
    public PaymentTypeGetDto ToDto(PaymentTypeEntity sender) =>
        new PaymentTypeGetDto
        {
            Id = sender.Id,
            Name = sender.Name
        };
}