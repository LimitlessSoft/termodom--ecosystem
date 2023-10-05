using TD.Core.Contracts.Dtos;
using TD.Core.Contracts.Http;
using TD.Web.Contracts.Requests.Images;

namespace TD.Web.Contracts.Interfaces.IManagers
{
    public interface IImagesManager
    {
        public Task<Response<string>> UploadAsync(ImagesUploadRequest request);
        public Task<Response<FileDto>> GetImageAsync(ImagesGetRequest request);
    }
}
