namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalApplication :
        Application
    {
        public InternalApplication(ApplicationImpl obj)
        {
            Name = obj.Name;
            Description = obj.Description;
            Version = obj.Version;
        }

        public string Name { get; }
        public string Description { get; }
        public string Version { get; }
    }
}