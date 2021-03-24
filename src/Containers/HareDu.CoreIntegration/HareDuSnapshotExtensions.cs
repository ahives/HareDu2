namespace HareDu.CoreIntegration
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Snapshotting;
    using Snapshotting.Persistence;

    public static class HareDuSnapshotExtensions
    {
        /// <summary>
        /// Registers all the necessary components to use the HareDu Snapshotting API.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHareDuSnapshot(this IServiceCollection services)
        {
            services.TryAddSingleton<ISnapshotFactory>(x =>
                new SnapshotFactory(x.GetService<IBrokerObjectFactory>()));
            
            services.TryAddSingleton<ISnapshotWriter, SnapshotWriter>();
            
            return services;
        }
    }
}