namespace HareDu.AutofacIntegration
{
    using Autofac;
    using Configuration;
    using Core;
    using Core.Configuration;
    using Shovel;

    public class HareDuShovelModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var settingsProvider = x.Resolve<IBrokerClientConfigProvider>();
                    var connection = x.Resolve<IBrokerConnectionClient>();

                    if (!settingsProvider.TryGet(out HareDuClientSettings settings))
                        throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");
                    
                    var client = connection.Create(settings);
                    
                    return new ShovelFactory(client);
                })
                .As<IShovelFactory>()
                .SingleInstance();

            builder.RegisterType<BrokerConnectionClient>()
                .As<IBrokerConnectionClient>()
                .SingleInstance();

            builder.RegisterType<BrokerClientConfigProvider>()
                .As<IBrokerClientConfigProvider>()
                .SingleInstance();
            
            base.Load(builder);
        }
    }
}