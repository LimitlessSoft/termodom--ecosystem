using LSCore.Common.Contracts;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using LSCore.SortAndPage.Domain;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Blogs;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Blogs;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Admin.Contracts.Helpers.Products;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class BlogManager(IBlogRepository repository) : IBlogManager
{
	public BlogsGetDto Get(LSCoreIdRequest request)
	{
		var blog = repository
			.GetMultiple()
			.Where(x => x.Id == request.Id)
			.FirstOrDefault();

		if (blog == null)
			throw new LSCoreNotFoundException();

		return blog.ToMapped<BlogEntity, BlogsGetDto>();
	}

	public LSCoreSortedAndPagedResponse<BlogsGetDto> GetMultiple(BlogsGetMultipleRequest request)
	{
		var query = repository
			.GetMultiple()
			.Where(x =>
				(
					string.IsNullOrWhiteSpace(request.SearchFilter)
					|| EF.Functions.ILike(x.Title, $"%{request.SearchFilter}%")
					|| EF.Functions.ILike(x.Slug, $"%{request.SearchFilter}%")
				)
				&& (
					request.Status == null
					|| request.Status.Length == 0
					|| request.Status.Contains(x.Status)
				)
			);

		return query.ToSortedAndPagedResponse<BlogEntity, BlogsSortColumnCodes.Blogs, BlogsGetDto>(
			request,
			BlogsSortColumnCodes.BlogsSortRules,
			x => x.ToMapped<BlogEntity, BlogsGetDto>()
		);
	}

	public long Save(BlogsSaveRequest request)
	{
		request.Validate();

		if (string.IsNullOrWhiteSpace(request.Slug))
			request.Slug = request.Title.GenerateSrc();

		var entity = request.Id is 0 or null
			? new BlogEntity()
			: repository.Get(request.Id!.Value);

		var previousStatus = entity.Status;
		entity.InjectFrom(request);

		if (previousStatus != BlogStatus.Published && entity.Status == BlogStatus.Published)
			entity.PublishedAt = DateTime.UtcNow;

		repository.UpdateOrInsert(entity);
		return entity.Id;
	}

	public void Delete(LSCoreIdRequest request)
	{
		var entity = repository.Get(request.Id);
		entity.IsActive = false;
		repository.Update(entity);
	}

	public void Publish(LSCoreIdRequest request)
	{
		var entity = repository.Get(request.Id);
		entity.Status = BlogStatus.Published;
		entity.PublishedAt = DateTime.UtcNow;
		repository.Update(entity);
	}

	public void Unpublish(LSCoreIdRequest request)
	{
		var entity = repository.Get(request.Id);
		entity.Status = BlogStatus.Draft;
		repository.Update(entity);
	}
}
