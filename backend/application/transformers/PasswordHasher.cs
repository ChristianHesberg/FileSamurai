using System.Security.Cryptography;

namespace application.transformers;

using Konscious.Security.Cryptography;
using System;
using System.Text;

public class PasswordHasher
{
    public static string HashPassword(string password, byte[] salt)
    {
        // Convert the password to bytes
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        // Use Argon2id for hashing
        using var argon2 = new Argon2id(passwordBytes);
        // Set parameters for Argon2
        argon2.Salt = salt; // A unique salt per user
        argon2.DegreeOfParallelism = 8; // Number of threads (CPU cores)
        argon2.MemorySize = 65536; // Memory size in KB (64 MB)
        argon2.Iterations = 4; // Number of iterations

        // Compute the hash
        var hashBytes = argon2.GetBytes(32); // Length of the resulting hash

        // Convert hash to a Base64 string for storage
        return Convert.ToBase64String(hashBytes);
    }

    public static byte[] GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(16);
    }

    public static bool VerifyPassword(string password, string storedHash, byte[] salt)
    {
        // Hash the provided password using the stored salt
        var newHash = HashPassword(password, salt);

        // Compare the hashes securely
        return storedHash == newHash;
    }
}