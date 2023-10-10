using Microsoft.AspNetCore.Http;
using TD.Core.Contracts.Dtos;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Web.Contracts.Requests.Images;

namespace TD.Web.Contracts.Interfaces.IManagers
{
    public interface IImageManager : IBaseManager
    {
        public Task<Response<string>> UploadAsync(ImagesUploadRequest request);
        public Task<FileResponse> GetImageAsync(ImagesGetRequest request);
    }
}
