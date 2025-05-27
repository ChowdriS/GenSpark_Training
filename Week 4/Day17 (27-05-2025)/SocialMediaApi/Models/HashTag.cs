using System;

namespace SocialMediaApi.Models;

public class HashTag
{
    public int Id { get; set; }
    public string Tag { get; set; } = string.Empty;

    public ICollection<TweetHashtag>? TweetHashTags { get; set; }
}

