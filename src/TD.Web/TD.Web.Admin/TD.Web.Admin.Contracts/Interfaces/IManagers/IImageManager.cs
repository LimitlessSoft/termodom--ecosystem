using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Admin.Contracts.Requests.Images;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IImageManager : ILSCoreBaseManager
    {
        Task<LSCoreResponse<string>> UploadAsync(ImagesUploadRequest request);
        Task<LSCoreFileResponse> GetImageAsync(ImagesGetRequest request);
    }
}
