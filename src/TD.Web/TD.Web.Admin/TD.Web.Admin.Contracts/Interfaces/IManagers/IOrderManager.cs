using TD.Web.Admin.Contracts.Requests.Orders;
using TD.Web.Admin.Contracts.Dtos.Orders;
using LSCore.Contracts.Responses;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IOrderManager
{
    LSCoreSortedAndPagedResponse<OrdersGetDto> GetMultiple(OrdersGetMultipleRequest request);
    OrdersGetDto GetSingle(OrdersGetSingleRequest request);
    void PutStoreId(OrdersPutStoreIdRequest request);
    void PutStatus(OrdersPutStatusRequest request);
    void PutPaymentTypeId(OrdersPutPaymentTypeIdRequest request);
    Task PostForwardToKomercijalnoAsync(OrdersPostForwardToKomercijalnoRequest request);
    void PutOccupyReferent(OrdersPutOccupyReferentRequest request);
    Task PostUnlinkFromKomercijalnoAsync(OrdersPostUnlinkFromKomercijalnoRequest request);
    void PutAdminComment(OrdersPutAdminCommentRequest request);
    void PutPublicComment(OrdersPutPublicCommentRequest request);
}