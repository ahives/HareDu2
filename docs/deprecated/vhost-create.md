# Create Virtual Host

The Broker API allows you to create a virtual host on the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<VirtualHost>()
    .Create(x =>
    {
        x.VirtualHost("vhost");
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<VirtualHost>()
    .Create(x =>
    {
        x.VirtualHost("vhost");
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<VirtualHost>()
    .Create(x =>
    {
        x.VirtualHost("vhost");
    });
```
<br>

If you want to enable tracing then you just need to call the ```WithTracingEnabled``` method within ```Configure``` like so...

```c#
c.WithTracingEnabled();
```
<br>

A complete example would look something like this...

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<VirtualHost>()
    .Create(x =>
    {
        x.VirtualHost("vhost");
        x.Configure(c =>
        {
            c.WithTracingEnabled();
        });
    });
```

<br>

*Please note that subsequent calls to any of the above methods will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md).

