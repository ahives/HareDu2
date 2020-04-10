# Creating Queues

The Broker API allows you to create a queue on the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the IoC way.

**Do It Yourself**

```csharp
var result = await new BrokerObjectFactory(config)
                .Object<Queue>()
                .Create(x =>
                {
                    x.Queue("your_queue");
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

**Autofac**

```csharp
var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create(x =>
                {
                    x.Queue("your_queue");
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

**.NET Core DI**

```csharp
var result = await _services.GetService<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create(x =>
                {
                    x.Queue("your_queue");
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

RabbitMQ supports the concept of [durability](https://www.rabbitmq.com/queues.html), which means that if the broker restarts the queues will survive. To configure a queue to be durable during creation, add the ```IsDurable``` method within ```Configure``` like so...

```csharp
c.IsDurable();
```
<br>

HareDu 2 supports the below RabbitMQ arguments during queue creation.

<br>

| Argument | Method |
| --- | --- |
| [x-expires](https://www.rabbitmq.com/ttl.html#queue-ttl) | SetQueueExpiration |
| [x-message-ttl](https://www.rabbitmq.com/ttl.html#message-ttl-using-policy) | SetPerQueuedMessageExpiration |
| [x-dead-letter-exchange](https://www.rabbitmq.com/dlx.html#using-optional-queue-arguments) | SetDeadLetterExchange |
| [x-dead-letter-routing-key](https://www.rabbitmq.com/dlx.html#using-optional-queue-arguments) | SetDeadLetterExchangeRoutingKey |
| [alternate-exchange](https://www.rabbitmq.com/ae.html) | SetAlternateExchange |

The addition of the below code to ```Configure``` will set the above RabbitMQ arguments.

```csharp
c.HasArguments(arg =>
{
    arg.SetQueueExpiration(1000);
    arg.SetAlternateExchange("your_alternate_exchange_name");
    arg.SetDeadLetterExchange("your_deadletter_exchange_name");
    arg.SetPerQueuedMessageExpiration(1000);
    arg.SetDeadLetterExchangeRoutingKey("your_routing_key");
});
```
<br>

A complete example would look something like this...

```csharp
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .Create(x =>
    {
        x.Queue("your_queue");
        x.Configure(c =>
        {
            c.IsDurable();
            c.HasArguments(arg =>
            {
                arg.SetQueueExpiration(1000);
                arg.SetAlternateExchange("your_alternate_exchange_name");
                arg.SetDeadLetterExchange("your_deadletter_exchange_name");
                arg.SetPerQueuedMessageExpiration(1000);
                arg.SetDeadLetterExchangeRoutingKey("your_routing_key");
            });
        });
        x.Targeting(t => t.VirtualHost("your_vhost"));
    });
```

<br>

*Please note that subsequent calls to any of the above methods will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/configuration.md) .

