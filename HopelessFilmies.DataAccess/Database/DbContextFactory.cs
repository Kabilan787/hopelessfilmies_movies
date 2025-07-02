using HopelessFilmiesMVC.DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace HopelessFilmies.DataAccess.Database;


    public class DBContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();

            // Row numbers for paging adds support for old SQL server 2005+. See more: 
            // https://learn.microsoft.com/en-us/ef/core/api/microsoft.entityframeworkcore.infrastructure.sqlserverdbcontextoptionsbuilder
            optionsBuilder.UseSqlServer("Server=.;Database=HopelessFilmiesDB_v2;Trusted_Connection=True;TrustServerCertificate=True;");

            return new UserDbContext(optionsBuilder.Options);
        }
    }


