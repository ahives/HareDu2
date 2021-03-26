# Delete Exchange

The Broker API allows you to delete an exchange from the RabbitMQ broker. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Exchange>()
    .Delete("exchange", "vhost");
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Exchange>()
    .Delete("exchange", "vhost");
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Exchange>()
    .Delete("exchange", "vhost");
```
<br>

Since deleting an exchange will cause messages to not be routed to queues, HareDu provides a means to conditional perform said action. You can delete an exchange when its not in use. You need only call the ```WhenUnused``` method like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Exchange>()
    .Delete("exchange", "vhost", x => x.WhenUnused());
```

<br>

The other way to delete an exchange is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .DeleteExchange("exchange", "vhost");
```

...or

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .DeleteExchange("exchange", "vhost", x => x.WhenUnused());
```

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

