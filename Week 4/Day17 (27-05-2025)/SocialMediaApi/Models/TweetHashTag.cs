using System;

namespace SocialMediaApi.Models;

public class TweetHashtag
{
    public int Id { get; set;}
    public int TweetId { get; set; }
    public Tweet? Tweet { get; set; }

    public int HashTagId { get; set; }
    public HashTag? HashTag { get; set; }
}
