namespace HareDu.AutofacIntegration
{
    using Autofac;
    using Snapshotting;
    using Snapshotting.Persistence;

    public static class HareDuSnapshotExtensions
    {
        public static ContainerBuilder AddHareDuSnapshot(this ContainerBuilder builder)
        {
            builder.Register(x => new SnapshotFactory(x.Resolve<IBrokerObjectFactory>()))
                .As<ISnapshotFactory>()
                .SingleInstance();
            
            builder.RegisterType<SnapshotWriter>()
                .As<ISnapshotWriter>()
                .SingleInstance();

            return builder;
        }
    }
}