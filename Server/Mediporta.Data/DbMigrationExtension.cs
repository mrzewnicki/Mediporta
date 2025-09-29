using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mediporta.Data;

public class MediportaDbContextFactory : IDesignTimeDbContextFactory<MediportaDbContext>
{
    public MediportaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MediportaDbContext>();
        optionsBuilder.UseSqlite("Data Source=Mediporta.db");

        return new MediportaDbContext(optionsBuilder.Options);
    }
}