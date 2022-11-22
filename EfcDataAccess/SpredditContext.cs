using Microsoft.EntityFrameworkCore;
using SharedDomain.Models;

namespace EfcDataAccess;

public class SpredditContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Vote> Votes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("DataSource = ../EfcDataAccess/Spreddit.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Username);
        });
        modelBuilder.Entity<Post>(entity =>
        {
            // a post has many comments
            entity.HasMany(post => post.Comments);
            entity.HasKey(post => post.Id);
            entity.HasOne(post => post.User);
        });
        modelBuilder.Entity<Comment>().HasKey(comment => comment.Id);
        modelBuilder.Entity<Vote>().HasKey(vote => new { vote.Username, vote.PostId });
    }

}