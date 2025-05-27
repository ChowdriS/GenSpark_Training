using System;

namespace SocialMediaApi.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    public ICollection<Tweet>? Tweet { get; set; }
    public ICollection<UserFollower>? Followers { get; set; }
    public ICollection<UserFollower>? Following { get; set; }
}

