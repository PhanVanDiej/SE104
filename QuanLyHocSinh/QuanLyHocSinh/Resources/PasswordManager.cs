using System.Security.Cryptography;
namespace QuanLyHocSinh.Resources
{
    public class PasswordManager
    {
        private const int SaltSize = 16; // 16 bytes (128 bits) salt
        private const int Iterations = 10000; // Number of iterations (recommended minimum is 10000)
        private const int DerivedKeyLength = 32; // 32 bytes (256 bits) derived key length

        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Create the Rfc2898DeriveBytes object and hash the password
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(DerivedKeyLength);

            // Combine salt and hash into a single string for storage
            byte[] hashBytes = new byte[SaltSize + DerivedKeyLength];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, DerivedKeyLength);

            // Convert to base64-encoded string for storage
            string base64Hash = Convert.ToBase64String(hashBytes);

            return base64Hash;
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Convert the base64-encoded string back to a byte array
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Extract the salt from the beginning of the hashBytes
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Compute the hash of the provided password using the same salt and iterations
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(DerivedKeyLength);

            // Compare the computed hash with the stored hash
            for (int i = 0; i < DerivedKeyLength; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false; // Passwords do not match
                }
            }

            return true; // Passwords match
        }
    }
}
