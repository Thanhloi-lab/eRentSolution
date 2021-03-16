using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace eRentSolution.Data.EF
{
    public class EShopDbContextFactory : IDesignTimeDbContextFactory<eRentDbContext>
    {
        public eRentDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("eRentSolutionDatabase");

            var optionsBuilder = new DbContextOptionsBuilder<eRentDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new eRentDbContext(optionsBuilder.Options);
        }
    }
}
