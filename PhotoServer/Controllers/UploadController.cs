using Microsoft.AspNetCore.Mvc;

namespace PhotoServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase // Controller to handle file uploads
    {
        private readonly IWebHostEnvironment _env; // To get the web root path

        public UploadController(IWebHostEnvironment env) // Constructor to inject IWebHostEnvironment
        {
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file) // Endpoint to handle file upload (POST /api/upload, gets hit when user selects upload button and calls handleUpload)
        {
            if (file == null || file.Length == 0) // Check if a file is ready to upload
                return BadRequest(new { error = "No file uploaded." });

            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" }; // Define allowed extensions
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant(); //Lowercase file extention for consistency

            if (extension == null) // Check if file has an extension
                return BadRequest(new { error = "File must have an extension." });

            if (!allowedExtensions.Contains(extension)) // Check if file extension is allowed
                return BadRequest(new { error = "Only .jpg and .png files are allowed." });

            var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads"); // Define upload folder path
            if (!Directory.Exists(uploadsFolder)) // Create upload folder if it doesn't exist (Will change to allow dynamic album folders later)
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create)) // Save the file to the server (into uploads folder)
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { message="Upload ok",fileName, url = $"/uploads/{fileName}" });
        }
    }
}
