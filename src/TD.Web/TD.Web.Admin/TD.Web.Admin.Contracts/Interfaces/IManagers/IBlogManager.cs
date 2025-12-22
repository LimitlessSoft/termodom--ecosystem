using LSCore.Common.Contracts;
using LSCore.SortAndPage.Contracts;
using TD.Web.Admin.Contracts.Dtos.Blogs;
using TD.Web.Admin.Contracts.Requests.Blogs;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IBlogManager
{
	BlogsGetDto Get(LSCoreIdRequest request);
	LSCoreSortedAndPagedResponse<BlogsGetDto> GetMultiple(BlogsGetMultipleRequest request);
	long Save(BlogsSaveRequest request);
	void Delete(LSCoreIdRequest request);
	void Publish(LSCoreIdRequest request);
	void Unpublish(LSCoreIdRequest request);
}
