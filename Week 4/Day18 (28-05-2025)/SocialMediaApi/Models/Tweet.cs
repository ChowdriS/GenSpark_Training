using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaApi.Models;

public class Tweet
{
    public int Id { get; set; }
    public string TweetContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    public ICollection<Like>? Likes { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<TweetHashtag>? TweetHashtags { get; set; }
}

