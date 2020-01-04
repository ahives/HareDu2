// Copyright 2013-2020 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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