# Get Nodes

The Broker API allows you to get all nodes on the RabbitMQ cluster. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Node>()
    .GetAll();
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Node>()
    .GetAll();
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Node>()
    .GetAll();
```
<br>

The other way to get consumer information is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .GetAllNodes();
```

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

