# Sync Queue

The Broker API allows you to sync queues. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Queue>()
    .Sync("queue", "vhost", QueueSyncAction.Sync);
```
<br>


**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .Sync("queue", "vhost", QueueSyncAction.Sync);
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Queue>()
    .Sync("queue", "vhost", QueueSyncAction.Sync);
```

The other way to get consumer information is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .SyncQueue("queue", "vhost");
```

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .CancelQueueSync("queue", "vhost");
```

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

