namespace HareDu.Extensions
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public static class PasswordHashExtensions
    {
        /// <summary>
        /// Given a string password will compute the hash based on algorithm found at http://www.rabbitmq.com/passwords.html#computing-password-hash
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string ComputePasswordHash(this string password)
        {
            byte[] salt = GetRandom32BitSalt();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] concatenatedBytes = salt.Concat(passwordBytes).ToArray();

            byte[] hash = GetSHA256Hash(concatenatedBytes);
            byte[] concatenatedHash = salt.Concat(hash).ToArray();

            string base64PasswordHash = Convert.ToBase64String(concatenatedHash);

            return base64PasswordHash;
        }

        static byte[] GetSHA256Hash(byte[] bytes)
        {
            var sha256 = new SHA256Managed();
            byte[] hash = sha256.ComputeHash(bytes);

            return hash;
        }

        static byte[] GetRandom32BitSalt()
        {
            var random = new RNGCryptoServiceProvider();
            
            byte[] salt = new byte[32];
            
            random.GetNonZeroBytes(salt);

            return salt;
        }
    }
}