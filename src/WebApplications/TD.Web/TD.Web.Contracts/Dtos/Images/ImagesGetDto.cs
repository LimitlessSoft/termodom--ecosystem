using Microsoft.AspNetCore.Http;

namespace TD.Web.Contracts.Dtos.Images
{
    public class ImagesGetDto
    {
        public IFormFile Image { get; set; }
        public string? AltText { get; set; }
    }
}