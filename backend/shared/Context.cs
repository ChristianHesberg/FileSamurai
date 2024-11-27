using core.models;
using Microsoft.EntityFrameworkCore;

namespace shared;

public class Context : DbContext
{
    protected Context()
    {
    }

    public Context(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserRsaKeyPair> UserRsaKeyPairs { get; set; }
    public DbSet<User> Users { get; set; }

}