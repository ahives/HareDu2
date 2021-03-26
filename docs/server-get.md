# Get Server Details

The Broker API allows you to get details on a RabbitMQ server. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<ServerInfo>()
    .Get();
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<ServerInfo>()
    .Get();
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<ServerInfo>()
    .Get();
```
<br>

The other way to get server information is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .GetServerInformation();
```

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/configuration.md) .

