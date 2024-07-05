using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Common.Contracts.Dtos;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface IImageManager
{
    Task<string> UploadAsync(ImagesUploadRequest request);
    Task<FileDto> GetImageAsync(ImagesGetRequest request);
}