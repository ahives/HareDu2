# Deleting Exchanges

The Broker API allows you to delete an exchange from the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the IoC way.

**Do It Yourself**

```csharp
var result = await new BrokerObjectFactory(config)
                .Object<Exchange>()
                .Delete(x =>
                {
                    x.Exchange("your_exchange");
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

**Autofac**

```csharp
var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Exchange>()
                .Delete(x =>
                {
                    x.Exchange("your_exchange");
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

**.NET Core DI**

```csharp
var result = await _services.GetService<IBrokerObjectFactory>()
                .Object<Exchange>()
                .Delete(x =>
                {
                    x.Exchange("your_exchange");
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

Since deleting an exchange will cause messages to not be routed to queues, HareDu provides a conditional way to perform said action. You can delete an exchange when its not in use. You need only add the ```When``` clause to the above code samples like so...

```csharp
.Delete(x =>
{
    x.Exchange("your_exchange");
    x.Targeting(t => t.VirtualHost("your_vhost"));
    x.When(c => c.Unused());
})
```
<br>

A complete example would look something like this...

```csharp
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Exchange>()
    .Delete(x =>
    {
        x.Exchange("your_exchange");
        x.Targeting(t => t.VirtualHost("your_vhost"));
        x.When(c => c.Unused());
    });
```

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/configuration.md) .

