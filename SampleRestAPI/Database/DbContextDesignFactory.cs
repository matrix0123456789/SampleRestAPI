using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SampleRestAPI.Database
{
    public class DbContextDesignFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySQL("server=localhost;database=SimpleRestApi;user=SimpleRestApi;password=12345678");


            return new AppDbContext(optionsBuilder.Options);
        }
    }
}