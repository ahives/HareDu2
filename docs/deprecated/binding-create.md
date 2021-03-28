# Create Binding

The Broker API allows you to create an exchange on the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the IoC way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Binding>()
    .Create(x =>
    {
        x.Binding(b =>
        {
            b.Source("source_exchange");
            b.Destination("destination_exchange");
            b.Type(BindingType.Exchange);
        });
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Binding>()
    .Create(x =>
    {
        x.Binding(b =>
        {
            b.Source("source_exchange");
            b.Destination("destination_exchange");
            b.Type(BindingType.Exchange);
        });
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Create(x =>
                {
                    x.Binding(b =>
                    {
                        b.Source("your_exchange");
                        b.Destination("your_exchange");
                        b.Type(BindingType.Exchange);
                    });
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

The above examples show how you would bind source exchange to a destination exchange. However, RabbitMQ allows you to bind an exchange to a queue. In this scenario, you would set the exchange as the source and the destination as the queue like so...


```c#
var result = await new BrokerObjectFactory(config)
    .Object<Binding>()
    .Create(x =>
    {
        x.Binding(b =>
        {
            b.Source("source_exchange");
            b.Destination("destination_exchange");
            b.Type(BindingType.Exchange);
        });
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

If you want to add a routing key to the binding then you just need to call the ```HasRoutingKey``` method within ```Configure``` like so...

```c#
c.HasRoutingKey("your_routing_key");
```
<br>

If you want to add adhoc arguments to the binding then you just need to call the ```Set``` method within ```HasArguments```, which is within ```Configure``` like so...

```c#
c.HasArguments(arg =>
{
    arg.Set("your_arg", "your_value");
});
```
<br>

A complete example would look something like this...

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Binding>()
    .Create(x =>
    {
        x.Binding(b =>
        {
            b.Source("source_exchange");
            b.Destination("destination_exchange");
            b.Type(BindingType.Queue);
        });
        x.Configure(c =>
        {
            c.HasRoutingKey("routing_key");
            c.HasArguments(arg =>
            {
                arg.Set("arg", "value");
            });
        });
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```

<br>

*Please note that subsequent calls to any of the above methods will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md).

