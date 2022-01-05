using Microsoft.EntityFrameworkCore;
using SampleRestAPI.Database.Entity;

namespace SampleRestAPI.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
