# Get Broker System Overview

The Broker API allows you to get broker system overview of the RabbitMQ broker. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<BrokerSystem>()
    .GetOverview()
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<BrokerSystem>()
    .GetOverview()
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<BrokerSystem>()
    .GetOverview()
```
<br>

The other way to get system overview information is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .GetSystemOverview();
```

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

