# Delete Exchange

The Broker API allows you to delete an exchange from the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Exchange>()
    .Delete(x =>
    {
        x.Exchange("exchange");
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Exchange>()
    .Delete(x =>
    {
        x.Exchange("exchange");
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Exchange>()
    .Delete(x =>
    {
        x.Exchange("exchange");
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

Since deleting an exchange will cause messages to not be routed to queues, HareDu provides a conditional way to perform said action. You can delete an exchange when its not in use. You need only add the ```When``` clause to the above code samples like so...

```c#
.Delete(x =>
{
    x.Exchange("exchange");
    x.Targeting(t => t.VirtualHost("vhost"));
    x.When(c => c.Unused());
})
```
<br>

A complete example would look something like this...

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Exchange>()
    .Delete(x =>
    {
        x.Exchange("exchange");
        x.Targeting(t => t.VirtualHost("vhost"));
        x.When(c => c.Unused());
    });
```

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md).

