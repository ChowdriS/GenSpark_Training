using AppointmentApi.Interface;
using AppointmentApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(FileUploadDto dto)
    {
        try
        {
            var fileName = await _fileService.UploadFileAsync(dto.File);
            return Ok(new { message = "File uploaded successfully", fileName });
        }
        catch
        {
            return BadRequest("Upload failed");
        }
    }

    [HttpGet("get")]
    public IActionResult Get(string fileName)
    {
        try
        {
            var content = _fileService.GetFile(fileName);
            return File(content, "application/octet-stream", fileName);
        }
        catch
        {
            return NotFound("File not found");
        }
    }
}
