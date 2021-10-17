using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Charis.Data.EF
{
    public class CharisDbContextFactory : IDesignTimeDbContextFactory<CharisDbContext>
    {
        public CharisDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsetting.json").Build();
            var optionsBuider = new DbContextOptionsBuilder<CharisDbContext>();
            var connectionString = configuration.GetConnectionString("CharisDb");
            optionsBuider.UseSqlServer(connectionString);
            return new CharisDbContext(optionsBuider.Options);
        }
    }
}