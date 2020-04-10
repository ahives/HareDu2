# Creating Exchanges

The Broker API allows you to create an exchange on the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the IoC way.

**Do It Yourself**

```csharp
var result = await new BrokerObjectFactory(config)
                .Object<Exchange>()
                .Create(x =>
                {
                    x.Exchange("your_exchange");
                    x.Configure(c =>
                    {
                        c.HasRoutingType(ExchangeRoutingType.Fanout);
                    });
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

**Autofac**

```csharp
var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Exchange>()
                .Create(x =>
                {
                    x.Exchange("your_exchange");
                    x.Configure(c =>
                    {
                        c.HasRoutingType(ExchangeRoutingType.Fanout);
                    });
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

**.NET Core DI**

```csharp
var result = await _services.GetService<IBrokerObjectFactory>()
                .Object<Exchange>()
                .Create(x =>
                {
                    x.Exchange("your_exchange");
                    x.Configure(c =>
                    {
                        c.HasRoutingType(ExchangeRoutingType.Fanout);
                    });
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

RabbitMQ supports the concept of [durability](https://www.rabbitmq.com/tutorials/amqp-concepts.html#exchanges), which means that if the broker restarts the exchange will survive. To configure an exchange to be durable during creation, add the ```IsDurable``` method within ```Configure``` like so...

```csharp
.Create(x =>
{
    x.Exchange("your_exchange");
    x.Configure(c =>
    {
        c.IsDurable();
        c.HasRoutingType(ExchangeRoutingType.Fanout);
    });
    x.Targeting(t => t.VirtualHost("your_vhost"));
})
```
<br>

There are situations where you wish to create exchanges that are bound to other exchanges and are not exposed to applications (i.e. publishers). In such a scenario, you can mark the exchange as internal like so...
```csharp
.Create(x =>
{
    x.Exchange("your_exchange");
    x.Configure(c =>
    {
        c.IsForInternalUse();
        c.HasRoutingType(ExchangeRoutingType.Fanout);
    });
    x.Targeting(t => t.VirtualHost("your_vhost"));
})
```
<br>

HareDu 2 supports adding adhoc arguments to exchanges during creation. The addition of the below code to ```Configure``` will set the above RabbitMQ arguments.

```csharp
c.HasArguments(arg =>
{
    arg.Set("your_arg", "your_value");
});
```
<br>

A complete example would look something like this...

```csharp
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Exchange>()
    .Create(x =>
    {
        x.Exchange("your_exchange");
        x.Configure(c =>
        {
            c.IsDurable();
            c.IsForInternalUse();
            c.HasRoutingType(ExchangeRoutingType.Fanout);
            c.HasArguments(arg =>
            {
                arg.Set("your_arg", "your_value");
            });
        });
        x.Targeting(t => t.VirtualHost("your_vhost"));
    });
```

<br>


*Please note that subsequent calls to any of the above methods will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/configuration.md) .

