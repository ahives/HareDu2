# Deleting Virtual Host Limits

The Broker API allows you to delete virtual host limits on a RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the IoC way.

**Do It Yourself**

```csharp
var result = await new BrokerObjectFactory(config)
                .Object<VirtualHostLimits>()
                .Delete(x => x.For("your_vhost"));
```
<br>

**Autofac**

```csharp
var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Delete(x => x.For("your_vhost"));
```
<br>

**.NET Core DI**

```csharp
var result = await _services.GetService<IBrokerObjectFactory>()
                .Object<VirtualHostLimits>()
                .Delete(x => x.For("your_vhost"));
```
<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/configuration.md) .

