using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Blogs;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Blogs;

public class BlogsGetDtoMappings : ILSCoreMapper<BlogEntity, BlogsGetDto>
{
	public BlogsGetDto ToMapped(BlogEntity sender)
	{
		var dto = new BlogsGetDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
