using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaApi.Models;

public class Like
{
    [Key]
    public int TweetId { get; set; }
    [ForeignKey("TweetId")]
    public Tweet? Tweet { get; set; }

    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }

    public DateTime LikedAt { get; set; }
}

