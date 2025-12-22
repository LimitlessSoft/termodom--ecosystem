using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;

namespace TD.Web.Admin.Contracts.Requests.Blogs;

public class BlogsGetMultipleRequest : LSCoreSortableAndPageableRequest<BlogsSortColumnCodes.Blogs>
{
	public string? SearchFilter { get; set; }
	public BlogStatus[]? Status { get; set; }
}
