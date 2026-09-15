using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BSI.SportsLive.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UploadsController : ControllerBase
    {
        private const long MaxFileSizeBytes = 500 * 1024; // 500 KB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly IWebHostEnvironment _env;

        public UploadsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("player-photo")]
        [RequestSizeLimit(MaxFileSizeBytes)]
        public async Task<IActionResult> UploadPlayerPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded." });

            if (file.Length > MaxFileSizeBytes)
                return BadRequest(new { message = "File too large. Maximum allowed size is 500 KB" });

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return BadRequest(new { message = "Invalid file type. Only JPG and PNG images are allowed." });

            var fileName = $"{Guid.NewGuid()}{extension}";
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "players");
            Directory.CreateDirectory(uploadsFolder); // safety net, folder na ho to bana de

            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"{Request.Scheme}://{Request.Host}/uploads/players/{fileName}";
            return Ok(new { url });
        }
    }
}