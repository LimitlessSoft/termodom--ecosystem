using TD.Core.Contracts.Http;
using TD.Web.Contracts.Dtos.Images;
using TD.Web.Contracts.Requests.Images;

namespace TD.Web.Contracts.Interfaces.IManagers
{
    public interface IImagesManager
    {
        public Task<Response<string>> UploadAsync(ImagesUploadRequest request);
        public Task<Response<ImagesGetDto>> GetImageAsync(ImagesGetRequest request);
    }
}
