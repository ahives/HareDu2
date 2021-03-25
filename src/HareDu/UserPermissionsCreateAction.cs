namespace HareDu
{
    using System;

    public interface UserPermissionsCreateAction
    {
        /// <summary>
        /// Specify the user for which the permission should be assigned to.
        /// </summary>
        /// <param name="username"></param>
        void User(string username);

        /// <summary>
        /// Define how the user permission is to be created.
        /// </summary>
        /// <param name="configure"></param>
        void Configure(Action<UserPermissionsConfiguration> configure);

        /// <summary>
        /// Specify the target for which the user permission will be created.
        /// </summary>
        /// <param name="target">Define which user is associated with the permissions that are being created.</param>
        void Targeting(Action<UserPermissionsTarget> target);
    }
}