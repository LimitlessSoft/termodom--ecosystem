using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;

namespace TD.Web.Common.Contracts.Requests.Cities;

public class GetMultipleCitiesRequest : LSCoreSortableRequest<CitiesSortColumnCodes.Cities>;
