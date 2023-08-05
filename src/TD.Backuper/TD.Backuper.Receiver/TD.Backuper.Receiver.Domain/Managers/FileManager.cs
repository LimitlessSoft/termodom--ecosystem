using Microsoft.Extensions.Logging;
using TD.Backuper.Receiver.Contracts;
using TD.Backuper.Receiver.Contracts.IManagers;
using TD.Backuper.Receiver.Contracts.Requests.Files;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;

namespace TD.Backuper.Receiver.Domain.Managers
{
    public class FileManager : BaseManager<FileManager>, IFileManager
    {
        public FileManager(ILogger<FileManager> logger) : base(logger)
        {
            Directory.CreateDirectory(Constants.FilesUploadsFolderPath);
        }

        public async Task<Response> Upload(UploadRequest request)
        {
            var response = new Response();
            if (request.IsRequestInvalid(response))
                return response;

            var finalFilePath = Path.Combine(Constants.FilesUploadsFolderPath, request.File.FileName);

            using(var fs = new FileStream(finalFilePath, FileMode.OpenOrCreate))
                await request.File.CopyToAsync(fs);

            return new Response();
        }
    }
}
