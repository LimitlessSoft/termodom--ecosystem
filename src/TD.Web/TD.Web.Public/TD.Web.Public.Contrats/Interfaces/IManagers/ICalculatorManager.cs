using TD.Web.Public.Contracts.Dtos.Calculator;
using TD.Web.Public.Contracts.Requests.Calculator;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface ICalculatorManager
{
    List<CalculatorItemDto> GetCalculatorItems(GetCalculatorItemsRequest request);
    CalculatorDto GetCalculator(GetCalculatorRequest request);
}
