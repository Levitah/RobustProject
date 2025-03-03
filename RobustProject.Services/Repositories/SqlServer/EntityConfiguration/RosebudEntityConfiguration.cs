using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RobustProject.Services.Entities;

namespace RobustProject.Services.Repositories.SqlServer.EntityConfiguration;

public class RosebudEntityConfiguration : IEntityTypeConfiguration<Rosebud>
{
    public RosebudEntityConfiguration()
    {

    }

    public void Configure(EntityTypeBuilder<Rosebud> builder)
    {
        builder
            .HasKey(x => x.Id);
    }
}