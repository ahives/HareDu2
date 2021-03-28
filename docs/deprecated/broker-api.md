# Broker API

The Broker API is the lowest level API because it interacts directly with the RabbitMQ broker. With this API you can administer the broker and perform the below operations on each broker object:

| Broker Object | Operations |
|---| --- |
| **Binding** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/binding-get.md), [Create](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/binding-create.md), [Delete](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/binding-delete.md) |
| **Channel** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/channel-get.md) |
| **Connection** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/connection-get.md) |
| **Consumer** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/consumer-get.md) |
| **Exchange** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/exchange-get.md), [Create](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/exchange-create.md), [Delete](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/exchange-delete.md) |
| **GlobalParameter** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/global-parameter-get.md), [Create](https://github.com/ahives/HareDu2/blob/master/docs/global-parameter-create.md), [Delete](https://github.com/ahives/HareDu2/blob/master/docs/global-parameter-delete.md) |
| **Node** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/node-get.md), [GetHealth](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/node-health.md), [GetMemoryUsage](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/node-memory-get.md) |
| **Policy** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/policy-get.md), [Create](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/policy-create.md), [Delete](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/policy-delete.md) |
| **Queue** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/queue-get.md), [Create](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/queue-create.md), [Delete](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/queue-delete.md), [Empty](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/queue-empty.md) |
| **ScopedParameter** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/scoped-parameter-get.md), [Create](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/scoped-parameter-create.md), [Delete](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/scoped-parameter-delete.md) |
| **Server**  | [Get](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/server-get.md), [GetHealth](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/server-health.md) |
| **SystemOverview** | [Get](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/system-overview-get.md) |
| **TopicPermissions** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/topic-permissions-get.md), [Create](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/topic-permissions-create.md), [Delete](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/topic-permissions-delete.md) |
| **User** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/user-get.md), [GetAllWithoutPermissions](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/user-get-without-permissions.md), [Create](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/user-create.md), [Delete](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/user-delete.md) |
| **UserPermissions** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/user-permissions-get.md), [Create](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/user-permissions-create.md), [Delete](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/user-permissions-delete.md) |
| **VirtualHost** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/vhost-get.md), [Create](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/vhost-create.md), [Delete](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/vhost-delete.md), [Startup](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/vhost-startup.md) |
| **VirtualHostLimits** | [GetAll](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/vhost-limits-get.md), [Define](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/vhost-limits-define.md), [Delete](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/vhost-limits-delete.md) |

#### Registering API objects
The very first thing you need to do is register/initialize the appropriate objects you will need to perform operations on the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported DI containers. Currently, HareDu 3 supports only two DI containers; Autofac and Microsoft, respectively.

#### Performing operations on the broker
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

Note: The above code will return a `Task<T>` so if you want to return the unwrapped ```Result```, ```Result<T>``` or ```ResultList``` you need to use an ```await``` or call the HareDu ```GetResult``` extension method.

Using the *async/await* pattern...
```c#
var result = await obj.GetAll();
```

Using the HareDu *GetResult* extension method...
```c#
var result = obj.GetAll().GetResult();
```

The above steps represent the minimum required code to get something up and working without an IoC container. However, if you want to use IoC then its even easier. Since HareDu is a fluent API, you can method chain everything together like so...

**Autofac**
```c#
var result = await container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .GetAll();
```

**Microsoft DI**
```c#
var result = await services.GetService<IBrokerObjectFactory>()
    .Object<Queue>()
    .GetAll();
```

<br>

*ex: Create a durable queue called *test-queue* on a vhost called *test-vhost* on node *rabbit@localhost* that has a per-message time to live (x-message-ttl) value of 2 seconds*

Here is the code...

```c#
var provider = new YamlFileConfigProvider();

provider.TryGet("haredu.yaml", out HareDuConfig config);

var factory = new BrokerObjectFactory(config);
var result = factory
    .Object<Queue>()
    .Create(x =>
    {
        x.Queue("HareDuQueue");
        x.Configure(c =>
        {
            c.IsDurable();
            c.AutoDeleteWhenNotInUse();
            c.HasArguments(arg =>
            {
                arg.SetQueueExpiration(5000);
                arg.SetPerQueuedMessageExpiration(2000);
            });
        });
        x.Targeting(t =>
        {
            t.VirtualHost("HareDu");
            t.Node("rabbit@localhost");
        });
    })
    .GetResult();
```
