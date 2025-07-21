using System;

namespace streamingApp.Models;

public class VideoAddRequestDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IFormFile? VideoFile { get; set; }
}
