using Coodesh.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coodesh.Challenge.Infrastructure.Database;

public sealed class DatabaseContext : DbContext
{
    private DatabaseContext(DbContextOptions options) : base(options) { }

    public static DatabaseContext Create(string dbStringConnection)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder();
        dbContextOptionsBuilder = dbContextOptionsBuilder.UseSqlServer(dbStringConnection);

        return new DatabaseContext(dbContextOptionsBuilder.Options);
    }

    public DbSet<Product> Product { get; set; }
}