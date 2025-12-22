using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.Blogs;

namespace TD.Web.Public.Contracts.DtoMappings.Blogs;

public class BlogGetDtoMappings : ILSCoreMapper<BlogEntity, BlogGetDto>
{
	public BlogGetDto ToMapped(BlogEntity sender)
	{
		var dto = new BlogGetDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
