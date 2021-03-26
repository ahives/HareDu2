# Broker API

The Broker API is the lowest level API because it interacts directly with the RabbitMQ broker. With this API you can administer the broker and perform the below operations on each broker object:

| Broker Object | Operations |
|---| --- |
| **Binding** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/binding-get.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/binding-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/binding-delete.md) |
| **Channel** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/channel-get.md) |
| **Connection** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/connection-get.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/connection-delete.md) |
| **Consumer** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/consumer-get.md) |
| **Exchange** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/exchange-get.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/exchange-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/exchange-delete.md) |
| **Queue** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/queue-get.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/queue-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/queue-delete.md), [Empty](https://github.com/ahives/HareDu3/blob/master/docs/queue-empty.md), [Sync](https://github.com/ahives/HareDu3/blob/master/docs/queue-sync.md) |
| **BrokerSystem** | [GetOverview](https://github.com/ahives/HareDu3/blob/master/docs/broker-system-overview-get.md), [RebalanceAllQueues](https://github.com/ahives/HareDu3/blob/master/docs/broker-system-rebalance-queues.md) |
| **VirtualHost** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/vhost-get.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/vhost-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/vhost-delete.md), [Startup](https://github.com/ahives/HareDu3/blob/master/docs/vhost-startup.md), [GetHealth](https://github.com/ahives/HareDu3/blob/master/docs/vhost-health.md) |
| **VirtualHostLimits** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/vhost-limits-get.md), [Define](https://github.com/ahives/HareDu3/blob/master/docs/vhost-limits-define.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/vhost-limits-delete.md) |
| **GlobalParameter** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/global-parameter-get.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/global-parameter-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/global-parameter-delete.md) |
| **Server**  | [Get](https://github.com/ahives/HareDu3/blob/master/docs/server-get.md) |
| **Node** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/node-get.md), [GetHealth](https://github.com/ahives/HareDu3/blob/master/docs/node-health.md), [GetMemoryUsage](https://github.com/ahives/HareDu3/blob/master/docs/node-memory-get.md) |
| **ScopedParameter** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/scoped-parameter-get.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/scoped-parameter-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/scoped-parameter-delete.md) |
| **TopicPermissions** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/topic-permissions-get.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/topic-permissions-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/topic-permissions-delete.md) |
| **User** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/user-get.md), [GetAllWithoutPermissions](https://github.com/ahives/HareDu3/blob/master/docs/user-get-without-permissions.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/user-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/user-delete.md) |
| **UserPermissions** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/user-permissions-get.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/user-permissions-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/user-permissions-delete.md) |
| **Policy** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/policy-get.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/policy-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/policy-delete.md) |
| **OperatorPolicy** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/operator-policy-get.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/operator-policy-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/operator-policy-delete.md) |
| **Shovel** | [GetAll](https://github.com/ahives/HareDu3/blob/master/docs/shovel-get.md), [Create](https://github.com/ahives/HareDu3/blob/master/docs/shovel-create.md), [Delete](https://github.com/ahives/HareDu3/blob/master/docs/shovel-delete.md) |

### Registering API Objects
The very first thing you need to do is register/initialize the appropriate objects you will need to perform operations on the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported DI containers. Currently, HareDu 3 supports only two DI containers; Autofac and Microsoft, respectively.

### Performing Broker Operations
The Broker API is considered the low level API because it allows you to administer RabbitMQ (e.g., users, queues, exchanges, etc.).

**Step 1: Get a broker object**
```c#
var factory = new BrokerObjectFactory(config);
var obj = factory.Object<Queue>();
```
Note: Initializing BrokerObjectFactory should be a one time activity, therefore, should be initialized using the Singleton pattern.

**Step 2: Call methods on broker object**
```c#
var result = obj.GetAll();
```

*Note: The above code will return a ```Task<T>``` so if you want to return the unwrapped ```Result```, ```Result<T>``` or ```ResultList``` you need to use an ```await``` or call the HareDu ```GetResult``` extension method.*

Using the *async/await* pattern...
```c#
var result = await obj.GetAll();
```

Using the HareDu *GetResult* extension method...
```c#
var result = obj.GetAll().GetResult();
```

The above steps represent the minimum required code to get something up and working without a DI container. However, if you want to use DI then its even easier. Since HareDu is a fluent API, you can method chain everything together like so...

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Binding>()
    .GetAll();
```

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Binding>()
    .GetAll();
```

*Note: if you have one of the above DI usage scenarios, then you can combine steps 1 and 2 because ```IBrokerObjectFactor``` is registered using the Singleton pattern and the ```Object``` method uses memoization and therefore will not take a performance hit for subsequent calls to the same object.*

<br>

*ex: Create a durable queue called *test-queue* on a vhost called *test-vhost* on node *rabbit@localhost* that has a per-message time to live (x-message-ttl) value of 2 seconds*

Here is the code...

```c#
var result = await services.GetService<IBrokerObjectFactory>()
    .Object<Queue>()
    .Create("test-queue", "test-vhost", "rabbit@localhost", x =>
    {
        x.IsDurable();
        x.HasArguments(arg =>
        {
            arg.SetPerQueuedMessageExpiration(2000);
        });
    });
```

Since HareDu 3 introduces extension methods for ```IBrokerObjectFactory```, you could rewrite the above code example like so...

```c#
var result = await services.GetService<IBrokerObjectFactory>()
    .CreateQueue("test-queue", "test-vhost", "rabbit@localhost", x =>
    {
        x.IsDurable();
        x.HasArguments(arg =>
        {
            arg.SetPerQueuedMessageExpiration(2000);
        });
    });
```
