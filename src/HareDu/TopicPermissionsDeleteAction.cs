namespace HareDu
{
    public interface TopicPermissionsDeleteAction
    {
        /// <summary>
        /// Specify the user for which the permission should be assigned to.
        /// </summary>
        /// <param name="username"></param>
        void User(string username);

        /// <summary>
        /// Specify the target for which the user permission will be created.
        /// </summary>
        /// <param name="target">Define which user is associated with the permissions that are being created.</param>
        void VirtualHost(string name);
    }
}