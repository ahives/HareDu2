# Broker API

The Broker API is the lowest level API because it interacts directly with the RabbitMQ broker. With this API you can administer the broker and perform the below operations on each broker object:

| Broker Object  | Operations |
|---| --- |
| **Binding** | GetAll, Create, Delete |
| **Channel** | GetAll |
| **Connection** | GetAll |
| **Consumer** | GetAll |
| **Exchange** | GetAll, Create, Delete |
| **GlobalParameter** | GetAll, Create, Delete |
| **Node** | GetAll, GetHealth, GetMemoryUsage |
| **Policy** | GetAll, Create, Delete |
| **Queue** | GetAll, Create, [Delete](https://github.com/ahives/HareDu2/blob/master/docs/queue-delete.md) , [Empty](https://github.com/ahives/HareDu2/blob/master/docs/queue-empty.md), [Peek](https://github.com/ahives/HareDu2/blob/master/docs/queue-peek.md) |
| **ScopedParameter** | GetAll, Create, Delete |
| **Server**  | Get, GetHealth |
| **SystemOverview** | Get |
| **TopicPermissions** | GetAll, Create, Delete |
| **User** | GetAll, GetAllWithoutPermissions, Create, Delete |
| **UserPermissions** | GetAll, Create, Delete |
| **VirtualHost** | GetAll, Create, Delete, Startup |
| **VirtualHostLimits** | GetAll, Define, Delete |

#### Registering API objects
The very first thing you need to do is register/initialize the appropriate objects you will need to perform operations on the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported IoC containers. Currently, HareDu 2 supports only two IoC containers; Autofac and .NET Core, respectively.

Note: The IoC container code that comes with HareDu currently defaults to file based configuration so you will need to make the appropriate changes to the haredu.yaml file.

<br>

#### Performing operations on the broker
The Broker API is considered the low level API because it allows you to administer RabbitMQ (e.g., users, queues, exchanges, etc.).

**Step 1: Get a broker object**
```csharp
var factory = new BrokerObjectFactory(config);
var obj = factory.Object<Queue>();
```
Note: Initializing BrokerObjectFactory should be a one time activity, therefore, should be initialized using the Singleton pattern.

**Step 2: Call methods on broker object**
```csharp
var result = obj.GetAll();
```

Note: The above code will return a `Task<T>` so if you want to return the unwrapped ```Result```, ```Result<T>``` or ```ResultList``` you need to use an ```await``` or call the HareDu ```GetResult``` extension method.

Using the *async/await* pattern...
```csharp
var result = await obj.GetAll();
```

Using the HareDu *GetResult* extension method...
```csharp
var result = obj.GetAll().GetResult();
```

The above steps represent the minimum required code to get something up and working without an IoC container. However, if you want to use IoC then its even easier. Since HareDu is a fluent API, you can method chain everything together like so...

*Autofac*
```csharp
var result = await container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .GetAll();
```

*.NET Core*
```csharp
var result = await services.GetService<IBrokerObjectFactory>()
    .Object<Queue>()
    .GetAll();
```

<br>

*ex: Create a durable queue called *HareDuQueue* on a vhost called *HareDu* on node *rabbit@localhost* that is deleted when not in use with per-message time to live (x-message-ttl) value of 2 seconds*

Here is the code...

```csharp
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
