namespace TD.Web.Public.Contracts.Dtos.Blogs;

public class BlogGetDto
{
	public long Id { get; set; }
	public string Title { get; set; }
	public string Slug { get; set; }
	public DateTime? PublishedAt { get; set; }
	public string? CoverImageContentType { get; set; }
	public string? CoverImageData { get; set; }
	public string? Summary { get; set; }
}
