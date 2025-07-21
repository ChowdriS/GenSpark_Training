using System;

namespace streamingApp.Models;

public class Video
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime UploadDate { get; set; }
    public string BlobUrl { get; set; } = string.Empty;
}

