# Create Queues

The Broker API allows you to create a queue on the RabbitMQ broker. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Queue>()
    .Create("queue", "vhost", "node");
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .Create("queue", "vhost", "node");
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Queue>()
    .Create("queue", "vhost", "node");
```
<br>

RabbitMQ supports the concept of [durability](https://www.rabbitmq.com/queues.html), which means that if the broker restarts the queues will survive. To configure a queue to be durable during creation, add the ```IsDurable``` method like so...

```c#
c.IsDurable();
```
<br>

HareDu 3 supports the below RabbitMQ arguments during queue creation.

<br>

| Argument | Method |
| --- | --- |
| [x-expires](https://www.rabbitmq.com/ttl.html#queue-ttl) | SetQueueExpiration |
| [x-message-ttl](https://www.rabbitmq.com/ttl.html#message-ttl-using-policy) | SetPerQueuedMessageExpiration |
| [x-dead-letter-exchange](https://www.rabbitmq.com/dlx.html#using-optional-queue-arguments) | SetDeadLetterExchange |
| [x-dead-letter-routing-key](https://www.rabbitmq.com/dlx.html#using-optional-queue-arguments) | SetDeadLetterExchangeRoutingKey |
| [alternate-exchange](https://www.rabbitmq.com/ae.html) | SetAlternateExchange |
| [x-max-length-bytes](https://www.rabbitmq.com/maxlength.html#definition-using-x-args) | SetMessageMaxSizeInBytes |
| [x-max-length](https://www.rabbitmq.com/maxlength.html#definition-using-x-args) | SetMaxMessagesPerQueue |
| [x-overflow](https://www.rabbitmq.com/maxlength.html#definition-using-x-args) | SetQueueOverflowBehavior |

The addition of the below code to ```Create``` will set the above RabbitMQ arguments.

```c#
c.HasArguments(arg =>
{
    arg.SetQueueExpiration(1000);
    arg.SetAlternateExchange("your_alternate_exchange_name");
    arg.SetDeadLetterExchange("your_deadletter_exchange_name");
    arg.SetPerQueuedMessageExpiration(1000);
    arg.SetDeadLetterExchangeRoutingKey("your_routing_key");
    arg.SetQueueOverflowBehavior();
    arg.SetMaxMessagesPerQueue(3);
    arg.SetMessageMaxSizeInBytes();
});
```
<br>

A complete example would look something like this...

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .Create("queue", "vhost", "node", x =>
    {
        x.IsDurable();
        x.AutoDeleteWhenNotInUse();
        x.HasArguments(arg =>
        {
            arg.SetQueueExpiration(1000);
            arg.SetAlternateExchange("your_alternate_exchange_name");
            arg.SetDeadLetterExchange("your_deadletter_exchange_name");
            arg.SetPerQueuedMessageExpiration(1000);
            arg.SetDeadLetterExchangeRoutingKey("your_routing_key");
        });
    });
```

<br>

The other way to create a policy is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .CreateQueue("queue", "vhost", "node", x =>
    {
        x.IsDurable();
        x.AutoDeleteWhenNotInUse();
        x.HasArguments(arg =>
        {
            arg.SetQueueExpiration(1000);
            arg.SetAlternateExchange("your_alternate_exchange_name");
            arg.SetDeadLetterExchange("your_deadletter_exchange_name");
            arg.SetPerQueuedMessageExpiration(1000);
            arg.SetDeadLetterExchangeRoutingKey("your_routing_key");
        });
    });
```

*Please note that subsequent calls to any of the above methods will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

