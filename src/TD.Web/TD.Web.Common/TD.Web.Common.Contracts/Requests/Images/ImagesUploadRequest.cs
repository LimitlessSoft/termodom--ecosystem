using Microsoft.AspNetCore.Http;

namespace TD.Web.Common.Contracts.Requests.Images
{
    public class ImagesUploadRequest
    {
        public IFormFile Image { get; set; }
        public string? AltText { get; set; }
    }
}
