using System;
using Microsoft.EntityFrameworkCore;
using streamingApp.Models;

namespace streamingApp.Context;

public class StreamContext : DbContext
{
    public StreamContext(DbContextOptions<StreamContext> options) : base(options) { }
    public DbSet<Video> Videos { get; set; }
}
