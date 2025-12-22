using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities;

public class BlogEntity : LSCoreEntity
{
	public string Title { get; set; }
	public string Text { get; set; }
	public string Slug { get; set; }
	public BlogStatus Status { get; set; }
	public DateTime? PublishedAt { get; set; }
	public string? CoverImage { get; set; }
	public string? Summary { get; set; }
}
