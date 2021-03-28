# Create Topic Permissions

The Broker API allows you to create topic permissions on the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<TopicPermissions>()
    .Create(x =>
    {
        x.User("username");
        x.VirtualHost("vhost");
        x.Configure(c =>
        {
            c.OnExchange("exchange");
            c.UsingReadPattern(".*");
            c.UsingWritePattern(".*");
        });
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<TopicPermissions>()
    .Create(x =>
    {
        x.User("username");
        x.VirtualHost("vhost");
        x.Configure(c =>
        {
            c.OnExchange("exchange");
            c.UsingReadPattern(".*");
            c.UsingWritePattern(".*");
        });
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<TopicPermissions>()
    .Create(x =>
    {
        x.User("username");
        x.VirtualHost("vhost");
        x.Configure(c =>
        {
            c.OnExchange("exchange");
            c.UsingReadPattern(".*");
            c.UsingWritePattern(".*");
        });
    });
```
<br>

*Please note that subsequent calls to any of the above methods within the Create method will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md).

