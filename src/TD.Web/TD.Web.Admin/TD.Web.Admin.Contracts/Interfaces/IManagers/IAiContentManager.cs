using TD.Web.Admin.Contracts.Dtos.AiContent;
using TD.Web.Admin.Contracts.Requests.AiContent;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IAiContentManager
{
	// Product validation
	Task<AiValidationResultDto> ValidateProductNameAsync(long productId, ValidateFieldRequest request);
	Task<AiValidationResultDto> ValidateProductDescriptionAsync(long productId, ValidateFieldRequest request);
	Task<AiValidationResultDto> ValidateProductShortDescriptionAsync(long productId, ValidateFieldRequest request);
	Task<AiValidationResultDto> ValidateProductMetaAsync(long productId, ValidateMetaRequest request);

	// Product generation
	Task<AiGeneratedContentDto> GenerateProductNameAsync(long productId, GenerateContentRequest request);
	Task<AiGeneratedContentDto> GenerateProductDescriptionAsync(long productId, GenerateContentRequest request);
	Task<AiGeneratedContentDto> GenerateProductShortDescriptionAsync(long productId, GenerateContentRequest request);
	Task<AiGeneratedContentDto> GenerateProductMetaAsync(long productId);
}
