using TD.Web.Common.Contracts.Requests.Images;
using LSCore.Contracts.Dtos;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface IImageManager
{
    Task<string> UploadAsync(ImagesUploadRequest request);
    Task<LSCoreFileDto> GetImageAsync(ImagesGetRequest request);
}