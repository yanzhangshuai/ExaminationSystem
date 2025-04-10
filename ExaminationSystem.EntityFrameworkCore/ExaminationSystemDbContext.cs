using ExaminationSystem.Model;
using ExaminationSystem.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.EntityFrameworkCore;


public class ExaminationSystemDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {

            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                entityType.AddSoftDeleteQueryFilter();
        }
        base.OnModelCreating(modelBuilder);
    }
    
}