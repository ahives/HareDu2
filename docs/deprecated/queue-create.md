# Create Queue

The Broker API allows you to create a queue on the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Queue>()
    .Create(x =>
    {
        x.Queue("queue");
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .Create(x =>
    {
        x.Queue("queue");
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Queue>()
    .Create(x =>
    {
        x.Queue("queue");
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

RabbitMQ supports the concept of [durability](https://www.rabbitmq.com/queues.html), which means that if the broker restarts the queues will survive. To configure a queue to be durable during creation, add the ```IsDurable``` method within ```Configure``` like so...

```c#
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

```c#
c.HasArguments(arg =>
{
    arg.SetQueueExpiration(1000);
    arg.SetAlternateExchange("alternate_exchange_name");
    arg.SetDeadLetterExchange("deadletter_exchange_name");
    arg.SetPerQueuedMessageExpiration(1000);
    arg.SetDeadLetterExchangeRoutingKey("routing_key");
});
```
<br>

A complete example would look something like this...

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .Create(x =>
    {
        x.Queue("queue");
        x.Configure(c =>
        {
            c.IsDurable();
            c.HasArguments(arg =>
            {
                arg.SetQueueExpiration(1000);
                arg.SetAlternateExchange("alternate_exchange_name");
                arg.SetDeadLetterExchange("deadletter_exchange_name");
                arg.SetPerQueuedMessageExpiration(1000);
                arg.SetDeadLetterExchangeRoutingKey("routing_key");
            });
        });
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```

<br>

*Please note that subsequent calls to any of the above methods will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md).

