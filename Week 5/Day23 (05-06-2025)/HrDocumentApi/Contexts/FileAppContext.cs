using HrDocumentApi.Models;
using System.Threading.Tasks;

namespace HrDocumentApi.Contexts;
public class HrDocumentApiContext : DbContext
{
    public HrDocumentApiContext(DbContextOptions<HrDocumentApiContext> options) : base(options)
    {
    }

    public DbSet<FileModel> Files { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileModel>().HasKey(f => f.Id);
        modelBuilder.Entity<FileModel>().HasOne(f => f.User)
                                        .WithMany(u => u.UploadedFiles)
                                        .HasForeignKey(f => f.UploadedBy)
                                        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>().HasKey(u => u.UserName);
    }
}