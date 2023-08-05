using Microsoft.AspNetCore.Http;

namespace TD.Backuper.Receiver.Contracts.Requests.Files
{
    public class UploadRequest
    {
        public IFormFile File { get; set; }
        public string Key { get; set; }
    }
}
