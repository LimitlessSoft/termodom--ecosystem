using FluentValidation;
using LSCore.Validation.Domain;
using TD.Web.Admin.Contracts.Enums;
using TD.Web.Admin.Contracts.Requests.Orders;

namespace TD.Web.Admin.Domain.Validators.Orders;

public class OrdersPostForwardToKomercijalnoRequestValidator
	: LSCoreValidatorBase<OrdersPostForwardToKomercijalnoRequest>
{
	public OrdersPostForwardToKomercijalnoRequestValidator()
	{
		RuleFor(x => x.DestinacioniMagacinId)
			.NotEmpty()
			.When(x => x.Type == ForwardToKomercijalnoType.InternaOtpremnica);
	}
}
