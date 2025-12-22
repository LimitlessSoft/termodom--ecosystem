using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Dtos.Blogs;

public class BlogsGetDto
{
	public long Id { get; set; }
	public string Title { get; set; }
	public string Text { get; set; }
	public string Slug { get; set; }
	public BlogStatus Status { get; set; }
	public DateTime? PublishedAt { get; set; }
	public string? CoverImage { get; set; }
	public string? Summary { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public bool IsActive { get; set; }
}
