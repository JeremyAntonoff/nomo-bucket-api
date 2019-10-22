using Microsoft.EntityFrameworkCore;
using nomo_bucket_api.Models;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<BucketListItem> BucketListItems { get; set; }
        public DbSet<FeedItem> FeedItems { get; set; }
        public DbSet<Follow> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Follow>()
            .HasKey(k => new { k.FollowerId, k.FolloweeId });

            builder.Entity<Follow>()
            .HasOne(u => u.Followee)
            .WithMany(u => u.Followers)
            .HasForeignKey(u => u.FolloweeId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follow>()
            .HasOne(u => u.Follower)
            .WithMany(u => u.Followees)
            .HasForeignKey(u => u.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}