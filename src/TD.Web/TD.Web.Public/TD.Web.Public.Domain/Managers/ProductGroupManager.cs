using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Public.Contracts;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.ProductsGroups;

namespace TD.Web.Public.Domain.Managers;

public class ProductGroupManager(IProductGroupRepository repository, ICacheManager cacheManager)
	: IProductGroupManager
{
	public ProductsGroupsGetDto Get(string src) =>
		repository
			.GetMultiple()
			.Include(x => x.ParentGroup)
			.FirstOrDefault(x => x.IsActive && x.Src.ToLower() == src.ToLower())
			?.ToMapped<ProductGroupEntity, ProductsGroupsGetDto>()
		?? throw new LSCoreNotFoundException();

	public List<ProductsGroupsGetDto> GetMultiple(ProductsGroupsGetRequest request)
	{
		return cacheManager
			.GetDataAsync(
				Constants.CacheKeys.ProductGroups(request),
				() =>
				{
					return repository
						.GetMultiple()
						.Include(x => x.ParentGroup)
						.Where(x =>
							(
								!request.ParentId.HasValue
									&& !string.IsNullOrWhiteSpace(request.ParentName)
								|| x.ParentGroupId == request.ParentId
							)
							&& (
								string.IsNullOrWhiteSpace(request.ParentName)
								|| x.ParentGroup != null
									&& x.ParentGroup.Name.ToLower() == request.ParentName.ToLower()
							)
						)
						.ToMappedList<ProductGroupEntity, ProductsGroupsGetDto>();
				},
				TimeSpan.FromDays(1)
			)
			.GetAwaiter()
			.GetResult();
	}
}
