# Defining Virtual Host Limits

The Broker API allows you to create a virtual host on the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<VirtualHostLimits>()
    .Define(x =>
    {
        x.VirtualHost("vhost");
        x.Configure(c =>
        {
            c.SetMaxQueueLimit(100);
            c.SetMaxConnectionLimit(1000);
        });
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<VirtualHostLimits>()
    .Define(x =>
    {
        x.VirtualHost("vhost");
        x.Configure(c =>
        {
            c.SetMaxQueueLimit(100);
            c.SetMaxConnectionLimit(1000);
        });
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<VirtualHostLimits>()
    .Define(x =>
    {
        x.VirtualHost("vhost");
        x.Configure(c =>
        {
            c.SetMaxQueueLimit(100);
            c.SetMaxConnectionLimit(1000);
        });
    });
```
<br>

*Please note that subsequent calls to any of the above methods will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md).

