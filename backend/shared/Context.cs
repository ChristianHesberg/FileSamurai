using System.Text;
using application;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SeedUserRsaKeyPair(modelBuilder);
    }

    private void SeedUserRsaKeyPair(ModelBuilder builder)
    {
        var cryptography = new Cryptography();
        var passwordToBytes = Encoding.UTF8.GetBytes("VerySecurePassword");
        var keyPair = cryptography.GenerateRsaKeyPair();
        var encryptionOutput = cryptography.Encrypt(keyPair.PrivateKey, passwordToBytes);
        var userKeyPair = new UserRsaKeyPair()
        {
            Id = Guid.NewGuid().ToString(),
            PublicKey = keyPair.PublicKey,
            PrivateKey = encryptionOutput.CipherText,
            Nonce = encryptionOutput.Nonce,
            Tag = encryptionOutput.Tag,
            Salt = encryptionOutput.Salt
        };
        builder.Entity<UserRsaKeyPair>().HasData(userKeyPair);
    }
}