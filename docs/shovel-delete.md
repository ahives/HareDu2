# Delete Shovel

The Broker API allows you to delete a dynamic shovel from RabbitMQ. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Shovel>()
    .Delete("shovel", "vhost");
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Shovel>()
    .Delete("shovel", "vhost");
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Shovel>()
    .Delete("shovel", "vhost");
```
<br>

The other way to create a policy is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .DeleteShovel("shovel", "vhost");
```
In the rare case when you need to delete all dynamic shovels on a particular virtual host, then, you can call the following extension method like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .DeleteAllShovels("vhost");
```

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

