using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;

namespace TD.Web.Common.Contracts.Requests.Stores;

public class GetMultipleStoresRequest : LSCoreSortableRequest<StoresSortColumnCodes.Stores>;
