using TD.Web.Common.Contracts.Dtos.PaymentTypes;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;

namespace TD.Web.Common.Contracts.DtoMappings.PaymentTypes
{
    public class PaymentTypesGetDtoMappings : ILSCoreDtoMapper<PaymentTypeEntity, PaymentTypesGetDto>
    {
        public PaymentTypesGetDto ToDto(PaymentTypeEntity sender) =>
            new PaymentTypesGetDto
            {
                Id = sender.Id,
                Name = sender.Name
            };
    }
}