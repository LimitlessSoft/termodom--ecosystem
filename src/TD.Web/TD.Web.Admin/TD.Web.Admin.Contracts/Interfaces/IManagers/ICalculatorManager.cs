using LSCore.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.Calculator;
using TD.Web.Admin.Contracts.Requests.Calculator;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface ICalculatorManager
{
    List<CalculatorItemDto> GetCalculatorItems(GetCalculatorItemsRequest request);
    void AddCalculatorItem(AddCalculatorItemRequest request);
    void RemoveCalculatorItem(RemoveCalculatorItemRequest request);
    void UpdateCalculatorItemQuantity(UpdateCalculatorItemQuantityRequest request);
    void MarkAsPrimaryCalculatorItem(LSCoreIdRequest request);
    void UpdateCalculatorItemUnit(UpdateCalculatorItemUnitRequest request);
    void MoveCalculatorItem(MoveCalculatorItemRequest request);
    void DeleteCalculatorItem(LSCoreIdRequest request);
    void UpdateCalculatorItemClassification(UpdateCalculatorItemClassificationRequest request);
}
