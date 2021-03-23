namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalTopicPermissionsInfo :
        TopicPermissionsInfo
    {
        public InternalTopicPermissionsInfo(TopicPermissionsInfoImpl obj)
        {
            User = obj.User;
            VirtualHost = obj.VirtualHost;
            Exchange = obj.Exchange;
            Write = obj.Write;
            Read = obj.Read;
        }

        public string User { get; }
        public string VirtualHost { get; }
        public string Exchange { get; }
        public string Write { get; }
        public string Read { get; }
    }
}