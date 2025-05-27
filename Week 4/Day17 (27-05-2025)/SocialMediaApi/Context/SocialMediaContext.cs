using System;
using Microsoft.EntityFrameworkCore;
using SocialMediaApi.Models;

namespace SocialMediaApi.Context;


public class SocialMediaContext : DbContext
{
    public SocialMediaContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Tweet> tweets { get; set; }
    public DbSet<Comment> comments { get; set; }
    public DbSet<HashTag> hashTags { get; set; }
    public DbSet<Like> likes { get; set; }
    public DbSet<TweetHashtag> tweetsHashtags { get; set; }
    public DbSet<User> users { get; set; }
    public DbSet<UserFollower> usersFollower { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserFollower>()
            .HasKey(uf => new { uf.FollowerId, uf.FolloweeId });

        modelBuilder.Entity<UserFollower>()
            .HasOne(uf => uf.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(uf => uf.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserFollower>()
            .HasOne(uf => uf.Followee)
            .WithMany(u => u.Followers)
            .HasForeignKey(uf => uf.FolloweeId)
            .OnDelete(DeleteBehavior.Restrict);
    }


}

