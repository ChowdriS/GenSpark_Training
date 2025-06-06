using FileApp.Services;
using FileApp.Misc;
using FileApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FileApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileOperationController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public FileOperationController(IFileService fileService,
                                    IHubContext<NotificationHub> hubContext)
    {
        _fileService = fileService;
        _hubContext = hubContext;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadedFile = await _fileService.UploadFile(file, User.Identity.Name);
            var message = $"{User.Identity.Name} uploaded a new file: {uploadedFile.FileName} ({uploadedFile.Size}) download link - http://localhost:5124/api/fileoperation/download/{uploadedFile.Id}";
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
            return Ok(uploadedFile);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error uploading file: {ex.Message}");

        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllFiles()
    {
        var files = await _fileService.GetAll();
        return Ok(files);
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadFile(int id)
    {
        var file = await _fileService.GetFile(id);
        if (file == null) return NotFound();

        var contentType = GetMimeType(file.FileType);
        return File(file.FileContent, contentType, file.FileName);
    }
    
    private string GetMimeType(string fileType)
    {
        return fileType.ToLower() switch
        {
            "pdf" => "application/pdf",
            "txt" => "text/plain",
            "md" => "text/markdown",
            "jpg" or "jpeg" => "image/jpeg",
            "png" => "image/png",
            "doc" => "application/msword",
            "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };
    }


}