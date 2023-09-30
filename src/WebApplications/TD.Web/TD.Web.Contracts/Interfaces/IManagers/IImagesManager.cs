using TD.Core.Contracts.Http;
using TD.Web.Contracts.Requests.Images;

namespace TD.Web.Contracts.Interfaces.IManagers
{
    public interface IImagesManager
    {
        public Response<string> Upload(ImagesUploadRequest request);
    }
}
