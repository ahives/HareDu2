# Get Scoped Parameters

The Broker API allows you to get all scoped parameters on the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<ScopedParameter>()
    .GetAll();
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<ScopedParameter>()
    .GetAll();
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<ScopedParameter>()
    .GetAll();
```
<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md).

