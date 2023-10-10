using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Web.Contracts.Requests.Images;

namespace TD.Web.Contracts.Interfaces.IManagers
{
    public interface IImageManager : IBaseManager
    {
        Task<Response<string>> UploadAsync(ImagesUploadRequest request);
        Task<FileResponse> GetImageAsync(ImagesGetRequest request);
    }
}
