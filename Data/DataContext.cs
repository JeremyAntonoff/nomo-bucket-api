using Microsoft.EntityFrameworkCore;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<BucketListItem> BucketListItems {get; set;}
  }
}