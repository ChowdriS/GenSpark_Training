using System;
using EventBookingApi.Model;
using Microsoft.EntityFrameworkCore;

namespace EventBookingApi.Context;

public class EventContext : DbContext
{
    public EventContext(DbContextOptions<EventContext> options): base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<TicketType> TicketTypes { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Email);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).HasMaxLength(200);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).IsRequired().HasMaxLength(50);
            entity.Property(u => u.IsDeleted).IsRequired();
            entity.Property(u => u.CreatedAt).IsRequired();
            entity.HasMany(u => u.ManagedEvents)
                  .WithOne(e => e.Manager)
                  .HasForeignKey(e => e.ManagerEmail)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.Tickets)
                  .WithOne(t => t.User)
                  .HasForeignKey(t => t.UserEmail)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Event
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.IsDeleted).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasMany(e => e.TicketTypes)
                  .WithOne(tt => tt.Event)
                  .HasForeignKey(tt => tt.EventId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Tickets)
                  .WithOne(t => t.Event)
                  .HasForeignKey(t => t.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TicketType
        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.HasKey(tt => tt.Id);
            entity.Property(tt => tt.TypeName).IsRequired().HasMaxLength(100);
            entity.Property(tt => tt.Price).IsRequired().HasColumnType("decimal(10,2)");
            entity.Property(tt => tt.TotalQuantity).IsRequired();
            entity.Property(tt => tt.BookedQuantity).IsRequired();
            entity.Property(tt => tt.Status).HasMaxLength(50);
            entity.Property(tt => tt.Description).HasMaxLength(500);
            entity.Property(tt => tt.CreatedAt).IsRequired();
        });

        // Ticket
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Status).HasMaxLength(50);
            entity.Property(t => t.BookedAt).IsRequired();
            entity.Property(t => t.IsDeleted).IsRequired();

            entity.HasOne(t => t.User)
                  .WithMany(u => u.Tickets)
                  .HasForeignKey(t => t.UserEmail)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.Event)
                  .WithMany(e => e.Tickets)
                  .HasForeignKey(t => t.EventId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.TicketType)
                  .WithMany()
                  .HasForeignKey(t => t.TicketTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}