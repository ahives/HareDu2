# Create Operator Policy

The Broker API allows you to create a operator policy on the RabbitMQ broker. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<OperatorPolicy>()
    .Create("policy", "pattern", "vhost", x =>
    {
        x.SetMaxInMemoryBytes(9803129);
        x.SetMaxInMemoryLength(283);
        x.SetDeliveryLimit(5);
        x.SetExpiry(5000);
        x.SetMessageMaxSize(189173219);
    }, OperatorPolicyAppliedTo.Queues, 0);
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<OperatorPolicy>()
    .Create("policy", "pattern", "vhost", x =>
    {
        x.SetMaxInMemoryBytes(9803129);
        x.SetMaxInMemoryLength(283);
        x.SetDeliveryLimit(5);
        x.SetExpiry(5000);
        x.SetMessageMaxSize(189173219);
    }, OperatorPolicyAppliedTo.Queues, 0);
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<OperatorPolicy>()
    .Create("policy", "pattern", "vhost", x =>
    {
        x.SetMaxInMemoryBytes(9803129);
        x.SetMaxInMemoryLength(283);
        x.SetDeliveryLimit(5);
        x.SetExpiry(5000);
        x.SetMessageMaxSize(189173219);
    }, OperatorPolicyAppliedTo.Queues, 0);
```
<br>

HareDu 3 supports the below RabbitMQ arguments during queue creation.

<br>

| Argument | Method |
| --- | --- |
| [expires](https://www.rabbitmq.com/ttl.html#queue-ttl) | SetExpiry |
| [message-ttl](https://www.rabbitmq.com/ttl.html#message-ttl-using-policy) | SetMessageTimeToLive |
| [max-length-bytes](https://www.rabbitmq.com/parameters.html#operator-policies) | SetMessageMaxSizeInBytes |
| [max-length](https://www.rabbitmq.com/parameters.html#operator-policies) | SetMessageMaxSize |
| [max-in-memory-bytes](https://www.rabbitmq.com/maxlength.html) | SetMaxInMemoryBytes |
| [max-in-memory-length](https://www.rabbitmq.com/maxlength.html) | SetMaxInMemoryLength |
| [delivery-limit](https://www.rabbitmq.com/blog/2020/04/20/rabbitmq-gets-an-ha-upgrade/) | SetDeliveryLimit |

<br>

The other way to create a policy is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .CreateOperatorPolicy("policy", "pattern", "vhost", x =>
    {
        x.SetMaxInMemoryBytes(9803129);
        x.SetMaxInMemoryLength(283);
        x.SetDeliveryLimit(5);
        x.SetExpiry(5000);
        x.SetMessageMaxSize(189173219);
    }, OperatorPolicyAppliedTo.Queues, 0);
```

*Please note that subsequent calls to any of the above methods will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

