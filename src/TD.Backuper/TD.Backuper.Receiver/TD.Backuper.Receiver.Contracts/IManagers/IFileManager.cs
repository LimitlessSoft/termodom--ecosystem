using TD.Backuper.Receiver.Contracts.Requests.Files;
using TD.Core.Contracts.Http;

namespace TD.Backuper.Receiver.Contracts.IManagers
{
    public interface IFileManager
    {
        Task<Response> Upload(UploadRequest request);
    }
}
