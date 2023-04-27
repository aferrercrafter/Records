using BallastLane.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BallastLane.Infrastructure.Persistence;

public class AppDBContext : DbContext
{
    public DbSet<Record> Records { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=MyDatabase.db");
    }
}
