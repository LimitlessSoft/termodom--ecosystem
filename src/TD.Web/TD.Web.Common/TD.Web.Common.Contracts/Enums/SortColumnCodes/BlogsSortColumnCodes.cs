using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Enums.SortColumnCodes;

public static class BlogsSortColumnCodes
{
	public enum Blogs
	{
		CreatedAt,
		PublishedAt,
		Title
	}

	public static Dictionary<Blogs, LSCoreSortRule<BlogEntity>> BlogsSortRules =
		new()
		{
			{ Blogs.CreatedAt, new LSCoreSortRule<BlogEntity>(x => x.CreatedAt) },
			{ Blogs.PublishedAt, new LSCoreSortRule<BlogEntity>(x => x.PublishedAt) },
			{ Blogs.Title, new LSCoreSortRule<BlogEntity>(x => x.Title) }
		};
}
