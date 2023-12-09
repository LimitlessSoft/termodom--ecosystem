using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Common.Contracts.Requests.Images;

namespace TD.Web.Common.Contracts.Interfaces.IManagers
{
    public interface IImageManager : ILSCoreBaseManager
    {
        Task<LSCoreResponse<string>> UploadAsync(ImagesUploadRequest request);
        Task<LSCoreFileResponse> GetImageAsync(ImagesGetRequest request);
    }
}
