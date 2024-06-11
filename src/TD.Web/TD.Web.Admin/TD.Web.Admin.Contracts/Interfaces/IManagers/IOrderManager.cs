using TD.Web.Admin.Contracts.Requests.Orders;
using TD.Web.Admin.Contracts.Dtos.Orders;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IOrderManager
{
    List<OrdersGetDto> GetMultiple(OrdersGetMultipleRequest request);
    OrdersGetDto GetSingle(OrdersGetSingleRequest request);
    void PutStoreId(OrdersPutStoreIdRequest request);
    void PutStatus(OrdersPutStatusRequest request);
    void PutPaymentTypeId(OrdersPutPaymentTypeIdRequest request);
    Task PostForwardToKomercijalnoAsync(OrdersPostForwardToKomercijalnoRequest request);
    void PutOccupyReferent(OrdersPutOccupyReferentRequest request);
    void PostUnlinkFromKomercijalno(OrdersPostUnlinkFromKomercijalnoRequest request);
}