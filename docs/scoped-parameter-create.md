# Create Scoped Parameter

The Broker API allows you to create a scoped parameter on the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<ScopedParameter>()
    .Create<long>("parameter", 89, "component", "vhost");
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<ScopedParameter>()
    .Create<long>("parameter", 89, "component", "vhost");
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<ScopedParameter>()
    .Create<long>("parameter", 89, "component", "vhost");
```
<br>

The other way to create a scoped parameter is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .CreateScopeParameter<long>("parameter", 89, "component", "vhost");
```

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

