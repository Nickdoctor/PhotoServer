using Microsoft.EntityFrameworkCore;
using PhotoServer.Models;

namespace PhotoServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
