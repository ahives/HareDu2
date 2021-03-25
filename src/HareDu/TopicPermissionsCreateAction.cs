namespace HareDu
{
    using System;

    public interface TopicPermissionsCreateAction
    {
        /// <summary>
        /// Specify the user for which the permission should be assigned to.
        /// </summary>
        /// <param name="username"></param>
        void User(string username);

        /// <summary>
        /// Define how the user permission is to be created.
        /// </summary>
        /// <param name="definition"></param>
        void Configure(Action<TopicPermissionsConfiguration> configure);

        /// <summary>
        /// Specify the target for which the user permission will be created.
        /// </summary>
        /// <param name="target">Define which user is associated with the permissions that are being created.</param>
        void VirtualHost(string name);
    }
}