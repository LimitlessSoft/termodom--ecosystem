using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Blogs;

public class BlogsSaveRequest
{
	public long? Id { get; set; }
	public string Title { get; set; }
	public string Text { get; set; }
	public string? Slug { get; set; }
	public BlogStatus Status { get; set; }
	public string? CoverImage { get; set; }
	public string? Summary { get; set; }
}
