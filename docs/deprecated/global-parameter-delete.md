# Delete Global Parameters

The Broker API allows you to delete global parameters on a RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<GlobalParameter>()
    .Delete(x => x.Parameter("param"));
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<GlobalParameter>()
    .Delete(x => x.Parameter("param"));
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<GlobalParameter>()
    .Delete(x => x.Parameter("param"));
```
<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

