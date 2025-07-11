using blobstorage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace blobstorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly BlobStorageService _blobStorageService;

        public BlobController(BlobStorageService blobStorageService)
        {
            _blobStorageService  = blobStorageService;
        }
        [HttpGet]
        public async Task<ActionResult<Stream>> Download(string fileName)
        {
            var stream = await _blobStorageService.DownloadFile(fileName);
            if (stream == null) 
                return NotFound();
            return File(stream, "application/octet-stream", fileName);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] FileDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file to upload");

            using var stream = dto.File.OpenReadStream();
            await _blobStorageService.UploadFile(stream, dto.File.FileName);

            return Ok("File uploaded");
        }

    }

}
