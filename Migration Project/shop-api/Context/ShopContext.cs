using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Models;

namespace shop_api.Context;

public class ShopContext : DbContext
{
    public ShopContext(DbContextOptions<ShopContext> options) : base(options)
    {
    }

    // public DbSet<CaptchaResponse> CaptchaResponses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<ContactU> ContactUs { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite key for OrderDetail
        modelBuilder.Entity<OrderDetail>()
            .HasKey(od => new { od.OrderID, od.ProductID });
        modelBuilder.Entity<Product>()
        .HasKey(p => p.ProductId);

        modelBuilder.Entity<Category>()
            .HasKey(c => c.CategoryId);

        modelBuilder.Entity<Color>()
            .HasKey(c => c.ColorId);

        modelBuilder.Entity<Model>()
            .HasKey(m => m.ModelId);

        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<Order>()
            .HasKey(o => o.OrderID);

        modelBuilder.Entity<News>()
            .HasKey(n => n.NewsId);

        // Relationships
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Color)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.ColorId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Model)
            .WithMany(m => m.Products)
            .HasForeignKey(p => p.ModelId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.User)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderID);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(od => od.ProductID);

        modelBuilder.Entity<News>()
            .HasOne(n => n.User)
            .WithMany(u => u.News)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        base.OnModelCreating(modelBuilder);
    }
}