using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using streamingApp.Context;
using streamingApp.Interface;
using streamingApp.Models;

namespace streamingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly StreamContext _context;
        private readonly IBlobService _blobService;

        public VideosController(StreamContext context, IBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] VideoAddRequestDTO dto)
        {
            try
            {
                if (dto.VideoFile == null || dto.VideoFile.Length == 0)
                    throw new Exception("No file uploaded.");

                var fileName = $"{dto.VideoFile.FileName}";
                using var stream = dto.VideoFile.OpenReadStream();
                var blobUrl = await _blobService.UploadFile(stream, fileName);

                var video = new Video
                {
                    Title = dto.Title ?? "",
                    Description = dto.Description ?? "",
                    UploadDate = DateTime.UtcNow,
                    BlobUrl = blobUrl
                };
                _context.Videos.Add(video);
                await _context.SaveChangesAsync();

                return Ok(video);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var videos = await _context.Videos
                    .OrderByDescending(v => v.UploadDate)
                    .ToListAsync();
                return Ok(videos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
