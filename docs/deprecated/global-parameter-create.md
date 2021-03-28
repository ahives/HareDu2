# Create Global Parameters

The Broker API allows you to create a simple global parameter on the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<GlobalParameter>()
    .Create(x =>
    {
        x.Parameter("param");
        x.Value("value");
    });
```
...or

```c#
var result = await new BrokerObjectFactory(config)
    .Object<GlobalParameter>()
    .Create(x =>
    {
        x.Parameter("param");
        x.Value(arg =>
        {
            arg.Set("arg1", "value");
            arg.Set("arg2", 5);
        });
    });
```

<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<GlobalParameter>()
    .Create(x =>
    {
        x.Parameter("param");
        x.Value("value");
    });
```
...or

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<GlobalParameter>()
    .Create(x =>
    {
        x.Parameter("param");
        x.Value(arg =>
        {
            arg.Set("arg1", "value");
            arg.Set("arg2", 5);
        });
    });
```

<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<GlobalParameter>()
    .Create(x =>
    {
        x.Parameter("param");
        x.Value("value");
    });
```
...or

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<GlobalParameter>()
    .Create(x =>
    {
        x.Parameter("param");
        x.Value(arg =>
        {
            arg.Set("arg1", "value");
            arg.Set("arg2", 5);
        });
    });
```

<br>

*Please note that subsequent calls to any of the above methods within Create will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/configuration.md).

