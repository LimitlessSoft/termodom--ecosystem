using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Requests.Images;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface IImageManager
{
	Task<string> UploadAsync(ImagesUploadRequest request);
	Task<FileDto> GetImageAsync(ImagesGetRequest request);
}
