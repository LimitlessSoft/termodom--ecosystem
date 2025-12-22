using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;

namespace TD.Web.Public.Contracts.Requests.Blogs;

public class GetMultipleBlogsRequest : LSCoreSortableAndPageableRequest<BlogsSortColumnCodes.Blogs>;
