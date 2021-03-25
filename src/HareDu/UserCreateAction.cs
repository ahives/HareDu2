namespace HareDu
{
    using System;

    public interface UserCreateAction
    {
        /// <summary>
        /// Specify the login username for the corresponding user.
        /// </summary>
        /// <param name="username"></param>
        void Username(string username);
        
        /// <summary>
        /// Specify the login password for the corresponding user.
        /// </summary>
        /// <param name="password"></param>
        void Password(string password);
        
        /// <summary>
        /// Specify the password for which a hash will eventually be computed.
        /// </summary>
        /// <param name="passwordHash"></param>
        void PasswordHash(string passwordHash);
        
        /// <summary>
        /// Specify the type of access the corresponding user has.
        /// </summary>
        /// <param name="tags"></param>
        void WithTags(Action<UserAccessOptions> tags);
    }
}