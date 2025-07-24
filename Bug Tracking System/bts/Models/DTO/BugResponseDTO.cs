using System;
using Bts.Models;

namespace bts.Models.DTO;

public class BugResponseDTO
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? ScreenshotUrl { get; set; }

    public BugPriority Priority { get; set; }

    public BugStatus Status { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string? AssignedTo { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<int>? ParentBugIds { get; set; }
}