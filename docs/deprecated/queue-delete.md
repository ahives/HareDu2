# Delete Queue

The Broker API allows you to delete a queue from the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Queue>()
    .Delete(x =>
    {
        x.Queue("queue");
        x.Targeting(l => l.VirtualHost("vhost"));
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .Delete(x =>
    {
        x.Queue("queue");
        x.Targeting(l => l.VirtualHost("vhost"));
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Queue>()
    .Delete(x =>
    {
        x.Queue("queue");
        x.Targeting(l => l.VirtualHost("vhost"));
    });
```
<br>

Since deleting a queue will also purge the queue of all messages as well, HareDu provides a conditional way to perform said action. You can delete a queue when there are no consumers and/or when the queue is empty. You need only add the ```When``` clause to the above code samples like so...

```c#
.Delete(x =>
{
    x.Queue("queue");
    x.Targeting(l => l.VirtualHost("vhost"));
    x.When(c =>
    {
        c.HasNoConsumers();
        c.IsEmpty();
    });
})
```
<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md).

