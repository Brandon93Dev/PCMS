using Microsoft.EntityFrameworkCore;
using PCMS_Backend.Models;

namespace PCMS_Backend.Infrastructure.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // in order for EF not to create a categories table
        modelBuilder.Ignore<Category>();
        base.OnModelCreating(modelBuilder);
    }
}
