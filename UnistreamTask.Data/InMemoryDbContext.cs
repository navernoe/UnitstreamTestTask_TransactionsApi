using Microsoft.EntityFrameworkCore;
using UnistreamTask.Data.Entities;

namespace UnistreamTask.Data;

public class InMemoryDbContext : DbContext
{
    public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
}