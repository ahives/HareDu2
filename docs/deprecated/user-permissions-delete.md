# Delete User Permissions

The Broker API allows you to delete user permissions from the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<UserPermissions>()
    .Delete(x =>
    {
        x.User("username");
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<UserPermissions>()
    .Delete(x =>
    {
        x.User("username");
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<UserPermissions>()
    .Delete(x =>
    {
        x.User("username");
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

*Please note that subsequent calls to any of the above methods within the Create method will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md).

