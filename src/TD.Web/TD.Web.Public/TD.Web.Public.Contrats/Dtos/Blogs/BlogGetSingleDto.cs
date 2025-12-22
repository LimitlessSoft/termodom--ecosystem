using TD.Web.Common.Contracts.Dtos;

namespace TD.Web.Public.Contracts.Dtos.Blogs;

public class BlogGetSingleDto
{
	public long Id { get; set; }
	public string Title { get; set; }
	public string Text { get; set; }
	public string Slug { get; set; }
	public DateTime? PublishedAt { get; set; }
	public FileDto? CoverImageData { get; set; }
	public string? Summary { get; set; }
}
