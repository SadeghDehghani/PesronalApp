using Microsoft.EntityFrameworkCore;
using PersonalAccounting.Models;

namespace PersonalAccounting.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "درآمد" },
            new Category { Id = 2, Name = "خوراک" },
            new Category { Id = 3, Name = "حمل و نقل" },
            new Category { Id = 4, Name = "قبوض" }
        );
    }
}