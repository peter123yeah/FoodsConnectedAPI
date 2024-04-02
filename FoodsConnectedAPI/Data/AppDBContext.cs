using FoodsConnectedAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodsConnectedAPI.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
        public DbSet<User> Users { get; set; }
    }
}
