using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaApi.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CommentedAt { get; set; }

    public int TweetId { get; set; }
    
    [ForeignKey("TweetId")]
    public Tweet? Tweet { get; set; }

    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
}


