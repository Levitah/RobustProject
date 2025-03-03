using Microsoft.EntityFrameworkCore;
using RobustProject.Services.Entities;

namespace RobustProject.Services.Repositories.SqlServer;

public class SqlServerDbContext : DbContext
{
    private readonly IEntityTypeConfiguration<Rosebud> _rosebudConfiguration;
    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options, IEntityTypeConfiguration<Rosebud> rosebudConfiguration)
        : base(options)
    {
        _rosebudConfiguration = rosebudConfiguration;
    }

    public DbSet<Rosebud> Rosebud { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Rosebud>().Property(e => e.Id).ValueGeneratedOnAdd();
        modelBuilder.ApplyConfiguration(_rosebudConfiguration);
    }
}