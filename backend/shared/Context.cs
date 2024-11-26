using core.models;
using Microsoft.EntityFrameworkCore;

namespace shared;

public class Context : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)  
    {  
        optionsBuilder.UseSqlite("Data Source=database.db");  
    }
    
    public DbSet<UserRsaKeyPair> UserRsaKeyPairs { get; set; }
}