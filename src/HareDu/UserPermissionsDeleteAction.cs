namespace HareDu
{
    using System;

    public interface UserPermissionsDeleteAction
    {
        /// <summary>
        /// Specify the user for which the permission should be deleted.
        /// </summary>
        /// <param name="name">User name</param>
        void User(string name);

        /// <summary>
        /// Specify the target for which the user permission will be deleted.
        /// </summary>
        /// <param name="target">Define what user will for which permissions will be targeted for deletion</param>
        void Targeting(Action<UserPermissionsTarget> target);
    }
}