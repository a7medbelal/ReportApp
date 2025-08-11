using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;

namespace  Linkoo.Common.Helper.PasswordServices
{
        public class PasswordHelper : IPasswordHelper
    {
            private readonly int _iterations = 10000;
            private readonly int _saltSize = 16;
            private readonly int _hashSize = 32;

            public (string salt, string hash) createPasswordHash(string password)
            {
                // Generate a cryptographically secure random salt
                var random = RandomNumberGenerator.Create();
                var salt = new byte[_saltSize];
                random.GetBytes(salt);

                // Use PBKDF2 to hash the password with the salt
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256);
                var hash = pbkdf2.GetBytes(_hashSize);

                var salted = Convert.ToBase64String(salt);
                var hashed = Convert.ToBase64String(hash);

                // Combine the iterations, salt, and hash into a single string
                var hashedPassword = $"{_iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
                return (salted, hashed);

            }

            public bool verifyPasswordHash(string password, string passwordHash, string salted)
            {

                // Extract the iterations, salt, and hash from the stored password hash
                var salt = Convert.FromBase64String(salted);
                var storedHash = Convert.FromBase64String(passwordHash);

                // Use PBKDF2 to hash the provided password with the same salt and iterations
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256);
                var computedHash = pbkdf2.GetBytes(_hashSize);

                // Compare the computed hash with the stored hash
                // return computedHash.SequenceEqual(storedHash);
                return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);

            }
        }
    }

