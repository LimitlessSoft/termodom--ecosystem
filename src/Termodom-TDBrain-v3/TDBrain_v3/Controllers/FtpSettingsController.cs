using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TDBrain_v3.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class FtpSettingsController : Controller
    {
        private class InfoDTO
        {
            public string? Url { get; set; }
            public string? Username { get; set; }
            public string? Password { get; set; }

            public InfoDTO(string? url, string? username, string? password)
            {
                this.Url = url;
                this.Username = username;
                this.Password = password;
            }
        }

        /// <summary>
        /// Vraca informacije o FTP-u
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/FTPSettings")]
        [Route("/ftpsettings/info")]
        public Task<IActionResult> Info()
        {
            return Task.Run<IActionResult>(() =>
            {
                return Json(new InfoDTO(FTPSettings.Url, FTPSettings.Username, FTPSettings.Password));
            });
        }
        /// <summary>
        /// Vraca username FTP-a
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/FTPSettings")]
        [Route("/ftpsettings/username/get")]
        public Task<IActionResult> UsernameGet()
        {
            return Task.Run<IActionResult>(() =>
            {
                return StatusCode(200, FTPSettings.Username);
            });
        }
        /// <summary>
        /// Vraca URL FTP-a
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/FTPSettings")]
        [Route("/ftpsettings/url/get")]
        public Task<IActionResult> UrlGet()
        {
            return Task.Run<IActionResult>(() =>
            {
                return StatusCode(200, FTPSettings.Url);
            });
        }
        /// <summary>
        /// Vraca sifru FTP-a
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/FTPSettings")]
        [Route("/ftpsettings/password/get")]
        public Task<IActionResult> PasswordGet()
        {
            return Task.Run<IActionResult>(() =>
            {
                return StatusCode(200, FTPSettings.Password);
            });
        }

        /// <summary>
        /// Azurira sifru FTP-a
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("/FTPSettings")]
        [Route("/ftpsettings/url/set")]
        public Task<IActionResult> UrlSet([FromForm][Required] string url)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    FTPSettings.Url = url;
                    Settings.Save();
                    return StatusCode(200);
                }
                catch(Exception ex)
                {
                    ex.Log();
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Azurira username FTP-a
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("/FTPSettings")]
        [Route("/ftpsettings/username/set")]
        public Task<IActionResult> UsernameSet([FromForm][Required] string username)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    FTPSettings.Username = username;
                    Settings.Save();
                    return StatusCode(200);
                }
                catch(Exception ex)
                {
                    ex.Log();
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Azurira sifru FTP-a
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("/FTPSettings")]
        [Route("/ftpsettings/password/set")]
        public Task<IActionResult> PasswordSet([FromForm][Required] string password)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(password))
                        return StatusCode(400, "Morate proslediti parametar 'password'!");

                    FTPSettings.Password = password;
                    Settings.Save();
                    return StatusCode(200);
                }
                catch(Exception ex)
                {
                    ex.Log();
                    return StatusCode(500);
                }
            });
        }
    }
}
