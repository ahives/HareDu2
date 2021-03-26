# Deleting Queues

The Broker API allows you to delete a queue from the RabbitMQ broker. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Queue>()
    .Delete("queue", "vhost");
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .Delete("queue", "vhost");
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Queue>()
    .Delete("queue", "vhost");
```
<br>

Since deleting a queue will also purge the queue of all messages as well, HareDu provides a conditional way to perform said action. You can delete a queue when there are no consumers and/or when the queue is empty. You need only call the ```WhenHasNoConsumers``` and/or the ```WhenEmpty``` method...

A complete example would look something like this...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Queue>()
    .Delete("queue", "vhost", x =>
    {
        x.WhenHasNoConsumers();
        x.WhenEmpty();
    });
```
<br>

The other way to delete a queue is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .DeleteQueue("queue", "vhost");
```

...or

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .DeleteQueue("queue", "vhost", x =>
    {
        x.WhenHasNoConsumers();
        x.WhenEmpty();
    });
```

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

