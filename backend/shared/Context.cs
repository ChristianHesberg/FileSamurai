using core.models;
using Microsoft.EntityFrameworkCore;
using File = core.models.File;

namespace shared;

public class Context : DbContext
{
    protected Context()
    {
    }

    public Context(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRsaKeyPair> UserRsaKeyPairs { get; set; }
    public DbSet<UserFileAccess> UserFileAccesses { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Group> Groups { get; set; }
}