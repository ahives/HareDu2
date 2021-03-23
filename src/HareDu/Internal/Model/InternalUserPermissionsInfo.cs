namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalUserPermissionsInfo :
        UserPermissionsInfo
    {
        public InternalUserPermissionsInfo(UserPermissionsInfoImpl obj)
        {
            User = obj.User;
            VirtualHost = obj.VirtualHost;
            Configure = obj.Configure;
            Write = obj.Write;
            Read = obj.Read;
        }

        public string User { get; }
        public string VirtualHost { get; }
        public string Configure { get; }
        public string Write { get; }
        public string Read { get; }
    }
}