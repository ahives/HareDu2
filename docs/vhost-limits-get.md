# Get Virtual Host Limits

The Broker API allows you to get all limits for all virtual hosts on the RabbitMQ broker. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<VirtualHostLimits>()
    .GetAll();
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<VirtualHostLimits>()
    .GetAll();
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<VirtualHostLimits>()
    .GetAll();
```
<br>

The other way to define virtual host limits is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .GetAllVirtualHostLimits();
```

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

