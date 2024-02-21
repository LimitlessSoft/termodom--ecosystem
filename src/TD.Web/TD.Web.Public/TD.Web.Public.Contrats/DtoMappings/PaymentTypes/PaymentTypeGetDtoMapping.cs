using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.PaymentTypes;

namespace TD.Web.Public.Contracts.DtoMappings.PaymentTypes
{
    public class PaymentTypeGetDtoMapping : ILSCoreDtoMapper<PaymentTypeGetDto, PaymentTypeEntity>
    {
        public PaymentTypeGetDto ToDto(PaymentTypeEntity sender) =>
            new PaymentTypeGetDto
            {
                Id = sender.Id,
                Name = sender.Name
            };
    }
}
