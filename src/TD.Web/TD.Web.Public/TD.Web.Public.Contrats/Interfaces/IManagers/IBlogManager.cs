using LSCore.SortAndPage.Contracts;
using TD.Web.Public.Contracts.Dtos.Blogs;
using TD.Web.Public.Contracts.Requests.Blogs;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface IBlogManager
{
	Task<LSCoreSortedAndPagedResponse<BlogGetDto>> GetMultipleAsync(GetMultipleBlogsRequest request);
	Task<BlogGetSingleDto> GetSingleAsync(GetSingleBlogRequest request);
}
