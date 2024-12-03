using core.models;
using Microsoft.EntityFrameworkCore;
using Document = System.Reflection.Metadata.Document;
using File = System.IO.File;

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
    public DbSet<Group> Groups { get; set; }
    public DbSet<core.models.File> Files { get; set; }
    public DbSet<UserGroupMembership> UserGroupMemberships { get; set; }
    public DbSet<UserFileAccess> UserFileAccesses { get; set; }

}