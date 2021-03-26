# Create Policy

The Broker API allows you to create a policy on the RabbitMQ broker. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Policy>()
    .Create("policy", "^amq.", "vhost", x =>
    {
        x.SetHighAvailabilityMode(HighAvailabilityModes.All);
        x.SetExpiry(1000);
    }, PolicyAppliedTo.All, 0);
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Policy>()
    .Create("policy", "^amq.", "vhost", x =>
    {
        x.SetHighAvailabilityMode(HighAvailabilityModes.All);
        x.SetExpiry(1000);
    }, PolicyAppliedTo.All, 0);
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Policy>()
    .Create("policy", "^amq.", "vhost", x =>
    {
        x.SetHighAvailabilityMode(HighAvailabilityModes.All);
        x.SetExpiry(1000);
    }, PolicyAppliedTo.All, 0);
```
<br>

HareDu 3 supports the below RabbitMQ arguments during queue creation.

<br>

| Argument | Method |
| --- | --- |
| [expires](https://www.rabbitmq.com/ttl.html#queue-ttl) | SetQueueExpiration |
| [message-ttl](https://www.rabbitmq.com/ttl.html#message-ttl-using-policy) | SetMessageTimeToLive |
| [dead-letter-exchange](https://www.rabbitmq.com/dlx.html#using-optional-queue-arguments) | SetDeadLetterExchange |
| [dead-letter-routing-key](https://www.rabbitmq.com/dlx.html#using-optional-queue-arguments) | SetDeadLetterExchangeRoutingKey |
| [alternate-exchange](https://www.rabbitmq.com/ae.html) | SetAlternateExchange |
| [federation-upstream](https://www.rabbitmq.com/parameters.html#policies) | SetFederationUpstream |
| [ha-mode](https://www.rabbitmq.com/parameters.html#policies) | SetHighAvailabilityMode |
| [ha-sync-mode](https://www.rabbitmq.com/ha.html#examples) | SetHighAvailabilitySyncMode |
| [ha-params](https://www.rabbitmq.com/ha.html) | SetHighAvailabilityParams |
| [queue-master-locator](https://www.rabbitmq.com/ha.html#queue-master-location) | SetQueueMasterLocator |
| [max-length-bytes](https://www.rabbitmq.com/parameters.html#operator-policies) | SetMessageMaxSizeInBytes |
| [max-length](https://www.rabbitmq.com/parameters.html#operator-policies) | SetMessageMaxSize |
| [ha-promote-on-shutdown](https://www.rabbitmq.com/ha.html#cluster-shutdown) | SetQueuePromotionOnShutdown |
| [ha-promote-on-failure](https://www.rabbitmq.com/ha.html#promoting-unsynchronised-mirrors) | SetQueuePromotionOnFailure |
| [ha-sync-batch-size](https://www.rabbitmq.com/ha.html#cluster-shutdown) | SetQueuedMessageSyncBatchSize |
| [queue-mode](https://www.rabbitmq.com/ha.html) | SetQueueMasterLocator |
| [delivery-limit](https://www.rabbitmq.com/blog/2020/04/20/rabbitmq-gets-an-ha-upgrade/) | SetDeliveryLimit |

<br>

The other way to create a policy is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .CreatePolicy("policy", "^amq.", "vhost", x =>
    {
        x.SetHighAvailabilityMode(HighAvailabilityModes.All);
        x.SetExpiry(1000);
    }, PolicyAppliedTo.All, 0);
```

*Please note that subsequent calls to any of the above methods will result in overriding the argument.*

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

