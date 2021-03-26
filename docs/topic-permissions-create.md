# Create Topic Permissions

The Broker API allows you to create topic permissions on the RabbitMQ broker. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<TopicPermissions>()
    .Create("username", "exchange", "vhost", x =>
    {
        x.UsingReadPattern(".*");
        x.UsingWritePattern(".*");
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<TopicPermissions>()
    .Create("username", "exchange", "vhost", x =>
    {
        x.UsingReadPattern(".*");
        x.UsingWritePattern(".*");
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<TopicPermissions>()
    .Create("username", "exchange", "vhost", x =>
    {
        x.UsingReadPattern(".*");
        x.UsingWritePattern(".*");
    });
```
<br>

The other way to create topic permissions is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .CreateTopicPermission("username", "exchange", "vhost", x =>
    {
        x.UsingReadPattern(".*");
        x.UsingWritePattern(".*");
    });
```

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

