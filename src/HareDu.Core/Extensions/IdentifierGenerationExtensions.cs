namespace HareDu.Core.Extensions
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class IdentifierGenerationExtensions
    {
        /// <summary>
        /// Generates a unique guid identifier based on the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetIdentifier(this Type type)
        {
            using (var algorithm = MD5.Create())
            {
                byte[] bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(type.FullName));
                
                return new Guid(bytes).ToString();
            }
        }
    }
}