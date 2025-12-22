using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using LSCore.SortAndPage.Domain;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Public.Contracts.Dtos.Blogs;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Blogs;

namespace TD.Web.Public.Domain.Managers;

public class BlogManager(
	IBlogRepository repository,
	IImageManager imageManager,
	ILogger<BlogManager> logger
) : IBlogManager
{
	public async Task<LSCoreSortedAndPagedResponse<BlogGetDto>> GetMultipleAsync(GetMultipleBlogsRequest request)
	{
		var query = repository
			.GetMultiple()
			.Where(x => x.IsActive && x.Status == BlogStatus.Published);

		var response = query.ToSortedAndPagedResponse<BlogEntity, BlogsSortColumnCodes.Blogs, BlogGetDto>(
			request,
			BlogsSortColumnCodes.BlogsSortRules,
			x => x.ToMapped<BlogEntity, BlogGetDto>()
		);

		// Load cover images for each blog
		var blogs = repository
			.GetMultiple()
			.Where(x => response.Payload.Select(i => i.Id).Contains(x.Id))
			.ToList();

		foreach (var dto in response.Payload)
		{
			var blog = blogs.FirstOrDefault(b => b.Id == dto.Id);
			if (blog?.CoverImage == null)
				continue;

			try
			{
				var imageResponse = await imageManager.GetImageAsync(
					new ImagesGetRequest
					{
						Image = blog.CoverImage,
						Quality = LegacyConstants.DefaultThumbnailQuality
					}
				);

				dto.CoverImageContentType = imageResponse.ContentType;
				dto.CoverImageData = Convert.ToBase64String(imageResponse.Data);
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "Failed to load cover image for blog {BlogId}", dto.Id);
			}
		}

		return response;
	}

	public async Task<BlogGetSingleDto> GetSingleAsync(GetSingleBlogRequest request)
	{
		var blog = repository
			.GetMultiple()
			.Where(x => x.IsActive && x.Status == BlogStatus.Published && x.Slug == request.Slug)
			.FirstOrDefault();

		if (blog == null)
			throw new LSCoreNotFoundException();

		var dto = blog.ToMapped<BlogEntity, BlogGetSingleDto>();

		// Load cover image
		if (!string.IsNullOrEmpty(blog.CoverImage))
		{
			try
			{
				dto.CoverImageData = await imageManager.GetImageAsync(
					new ImagesGetRequest
					{
						Image = blog.CoverImage,
						Quality = LegacyConstants.DefaultImageQuality
					}
				);
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "Failed to load cover image for blog {BlogId}", blog.Id);
			}
		}

		return dto;
	}
}
