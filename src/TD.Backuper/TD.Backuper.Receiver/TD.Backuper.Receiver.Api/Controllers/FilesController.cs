using Microsoft.AspNetCore.Mvc;
using TD.Backuper.Receiver.Contracts.IManagers;
using TD.Backuper.Receiver.Contracts.Requests.Files;
using TD.Core.Contracts.Http;

namespace TD.Backuper.Receiver.Api.Controllers
{
    [ApiController]
    public class FilesController : Controller
    {
        private readonly IFileManager _fileManager;
        public FilesController(IFileManager fileManger)
        {
            _fileManager = fileManger;
        }

        [HttpPost]
        [Route("/upload")]
        public async Task<Response> Upload([FromForm]UploadRequest request)
        {
            return await _fileManager.Upload(request);
        }
    }
}
