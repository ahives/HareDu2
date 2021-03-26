# Start Up Virtual Host

The Broker API allows you to start up a virtual host on the RabbitMQ node. To do so is pretty simple with HareDu 3. You can do it yourself or the IoC way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<VirtualHost>()
    .Startup("vhost", "node");
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<VirtualHost>()
    .Startup("vhost", "node");
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<VirtualHost>()
    .Startup("vhost", "node");
```
<br>

The other way to startup a virtual host is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .StartupVirtualHost("vhost", "node");
```

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

