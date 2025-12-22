using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.Blogs;

namespace TD.Web.Public.Contracts.DtoMappings.Blogs;

public class BlogGetSingleDtoMappings : ILSCoreMapper<BlogEntity, BlogGetSingleDto>
{
	public BlogGetSingleDto ToMapped(BlogEntity sender)
	{
		var dto = new BlogGetSingleDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
