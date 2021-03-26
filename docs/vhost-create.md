# Create Virtual Host

The Broker API allows you to create a virtual host on the RabbitMQ broker. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<VirtualHost>()
    .Create("vhost", x =>
    {
        x.WithTracingEnabled();
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<VirtualHost>()
    .Create("vhost", x =>
    {
        x.WithTracingEnabled();
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<VirtualHost>()
    .Create("vhost", x =>
    {
        x.WithTracingEnabled();
    });
```
<br>

If you want to enable tracing then you just need to call the ```WithTracingEnabled``` method like so...

```c#
c.WithTracingEnabled();
```
<br>

The other way to create a virtual host is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .CreateVirtualHost("vhost", x =>
    {
        x.WithTracingEnabled();
    });
```

<br>

*Please note that subsequent calls to any of the above methods will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

