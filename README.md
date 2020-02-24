# HareDu 2

![Join the chat at https://gitter.im/HareDu2/Lobby](https://img.shields.io/gitter/room/haredu2/HareDu2?style=flat)
![NuGet downloads](https://img.shields.io/nuget/dt/haredu?style=flat)

.NET API for managing and monitoring RabbitMQ clusters using the RabbitMQ RESTful API.


Docs under construction here

https://ahives.gitbooks.io/haredu2/content/

# History

HareDu 2 is a complete rewrite of the original HareDu 1.x API. This rewrite came about from feedback of production deployments and because the original API was lacking in some key areas. In particular, HareDu 2 introduces the following enhancements:
1. Increased test coverage
2. Improved low level administrative APIs
3. New APIs for diagnostics, cluster snapshotting and monitoring
4. Dependency Injection (e.g., Autofac, .NET Core) support for quick API registration
5. .NET Core support 


# Overview

HareDu 2 comes with three major APIs; that is, Broker, Snapshotting, and Diagnostics, respectively.  

To use HareDu, you must have the appropriate RabbitMQ plugins installed and enabled. Ensure that you have the following configuration values set to enable the API to pull statistics from your RabbitMQ cluster:

Configuration value "vm_memory_calculation_strategy" is set to "rss"

### Broker API

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
| **Queue** | GetAll, Create, Delete, Empty, Peek |
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

**Autofac**
```csharp
builder.RegisterModule<HareDuModule>();
```

**.NET Core DI**
```csharp
var services = new ServiceCollection()
    .AddHareDu()
    .BuildServiceProvider();
```

Note: The IoC container code that comes with HareDu currently defaults to file based configuration so you will need to make the appropriate changes to the haredu.yaml file.

<br>

Registering objects without IoC containers is pretty simple as well...

*YAML configuration*
```csharp
var provider = new YamlConfigProvider();

provider.TryGet("haredu.yaml", out HareDuConfig config);

var factory = new BrokerObjectFactory(config.Broker);
```

*Programmatically*
```csharp
var provider = new BrokerConfigProvider();
var config = provider.Configure(x =>
{
    x.ConnectTo("http://localhost:15672");
    x.UsingCredentials("guest", "guest");
});

var factory = new BrokerObjectFactory(config);
```
Note: Initializing BrokerObjectFactory should be a one time activity, therefore, should be initialized using the Singleton pattern.

#### Performing operations on the broker
The Broker API is considered the low level API because it allows you to administer RabbitMQ (e.g., users, queues, exchanges, etc.).

**Step 1: Get a broker object**
```csharp
var obj = factory.Object<Queue>();
```

**Step 2: Call methods on broker object**
```csharp
var result = obj.GetAll();
```

Note: The above code will return a `Task<T>` so if you want to return the unwrapped ```Result```, ```Result<T>``` or ```ResultList``` you need to use an ```await``` or call the HareDu ```Unfold``` extension method.

Using the *async/await* pattern...
```csharp
var result = await obj.GetAll();
```

Using the HareDu *Unfold* extension method...
```csharp
var result = obj.GetAll().Unfold();
```

The above steps represent the minimum required code to get something up and working without an IoC container. However, if you want to use IoC then its even easier. Since HareDu is a fluent API, you can method chain everything together like so...

**Autofac**
```csharp
var result = await container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .GetAll();
```

**.NET Core**
```csharp
var result = await services.GetService<IBrokerObjectFactory>()
    .Object<Queue>()
    .GetAll();
```

<br>

*ex: Create a durable queue called *HareDuQueue* on a vhost called *HareDu* on node *rabbit@localhost* that is deleted when not in use with per-message time to live (x-message-ttl) value of 2 seconds*

Here is the code...

```csharp
var result = await container.Resolve<IBrokerObjectFactory>()
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
                });
```


### Snapshot API

The Snapshot API sits atop the Broker API and provides a high level rollup of RabbitMQ broker metrics. Each snapshot makes one or more calls to the Broker API methods aggregating the metric data into a developer-friendly object. Each snapshot is then captured on a timeline that can be then flushed to disk or saved to a database.

#### Registering API objects
The very first thing you need to do is register/initialize the appropriate objects you will need to take snapshots metric data on the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported IoC containers. Currently, HareDu 2 supports only two IoC containers; Autofac and .NET Core, respectively.

**Autofac**
```csharp
builder.RegisterModule<HareDuSnapshotModule>();
```

**.NET Core DI**
```csharp
var services = new ServiceCollection()
    .AddHareDuSnapshot()
    .BuildServiceProvider();
```

Note: The IoC container code that comes with HareDu currently defaults to file based configuration so you will need to make the appropriate changes to the haredu.yaml file.

<br>

Registering objects without IoC containers is pretty simple as well...

*YAML configuration*
```csharp
var provider = new YamlConfigProvider();

provider.TryGet("haredu.yaml", out HareDuConfig config);

var brokerFactory = new BrokerObjectFactory(config.Broker);
var factory = new SnapshotFactory(brokerFactory);
```

*Programmatically*
```csharp
var provider = new BrokerConfigProvider();
var config = provider.Configure(x =>
{
    x.ConnectTo("http://localhost:15672");
    x.UsingCredentials("guest", "guest");
});

var brokerFactory = new BrokerObjectFactory(config);
var factory = new SnapshotFactory(brokerFactory);
```
Note: if you have the broker configuration already you can simply pass it to the SnapshotFactory instead of explicitly initializing BrokerObjectFactory.

<br>

#### Taking snapshots
Once you have registered a SnapshotFactory, it is easy to take a snapshot.

**Step 1: Define which snapshot you want to take**
```csharp
var snapshot = factory.Snapshot<BrokerQueues>();
```

**Step 2: Take the snapshot**
```csharp
snapshot.Execute();
```

<br>

*The above code becomes even simpler using an IoC container. Below is how you would take a snapshot on the first take...*

**Autofac**
```csharp
var result = await container.Resolve<ISnapshotFactory>()
    .Snapshot<BrokerQueues>()
    .Execute();
```

**.NET Core DI**
```csharp
var result = await services.GetService<ISnapshotFactory>()
    .Snapshot<BrokerQueues>()
    .Execute();
```
<br>

#### Viewing snapshots

Snapshots are accessible via the Timeline property on the SnapshotFactory. Getting the most recent snapshot is as easy as calling the MostRecent extension method like so...  

```csharp
var snapshot = factory.Timeline.MostRecent();
```

#### Registering Observers

When setting up the SnapshotFactory, you can register observers. On each time a snapshot is taken (i.e. when the Execute method is called), all registered observers will be notified. Registering an observer is simple.

```csharp
var snapshot = factor
    .Snapshot<BrokerQueues>()
    .RegisterObserver(new SomeCoolObserver())
    .Execute();
```

### Diagnostics API

The Diagnostics API sits atop the Snapshot API, providing a means to scan snapshot data for issues.  
Each snapshot makes one or more calls to the Broker API methods aggregating the metric data into a developer-friendly object. Each snapshot is then captured on a timeline that can be then flushed to disk or saved to a database.

#### Registering API objects
The very first thing you need to do is register/initialize the appropriate objects you will need to perform diagnostic scans on snapshot data captured from the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported IoC containers. Currently, HareDu 2 supports only two IoC containers; Autofac and .NET Core, respectively.

**Autofac**
```csharp
builder.RegisterModule<HareDuDiagnosticsModule>();
```

**.NET Core DI**
```csharp
var services = new ServiceCollection()
    .AddHareDuDiagnostics()
    .BuildServiceProvider();
```

Note: The IoC container code that comes with HareDu currently defaults to file based configuration so you will need to make the appropriate changes to the haredu.yaml file.

<br>

Registering objects without IoC containers is pretty simple...

*YAML configuration*
```csharp
var provider = new YamlConfigProvider();

provider.TryGet("haredu.yaml", out HareDuConfig config);

var kb = new DefaultKnowledgeBaseProvider();
var scanner = new DiagnosticScanner(config.Diagnostics, new DefaultKnowledgeBaseProvider());
```

*Programmatically*
```csharp
var provider = new DiagnosticsConfigProvider();
var config = provider.Configure(x =>
            {
                x.SetMessageRedeliveryCoefficient(0.60M);
                x.SetSocketUsageCoefficient(0.60M);
                x.SetConsumerUtilizationWarningCoefficient(0.65M);
                x.SetQueueHighFlowThreshold(90);
                x.SetQueueLowFlowThreshold(10);
                x.SetRuntimeProcessUsageCoefficient(0.65M);
                x.SetFileDescriptorUsageWarningCoefficient(0.65M);
                x.SetHighClosureRateWarningThreshold(90);
                x.SetHighCreationRateWarningThreshold(60);
            });

var kb = new DefaultKnowledgeBaseProvider();
var scanner = new DiagnosticScanner(config.Diagnostics, new DefaultKnowledgeBaseProvider());
```

#### Scanning snapshots

```csharp
var result = scanner.Scan(snapshot);
```

The above code will return a ```ScannerResult``` object, which contains the result of executing each diagnostic probe against the snapshot data.


### Gotchas

Registering an API will register all dependent objects so there is no need to include them. So, if you intend on using both the Broker and Snapshot APIs then including the Snapshot API will also register everything in the Broker API. The same can be said for the Diagnostic API, since it is dependent on both the Broker and Snapshot APIs.


# Get It
You can now get HareDu 2 on NuGet by searching for HareDu. Also, you can check out HareDu at https://github.com/ahives/HareDu2

From the Package Manager Console in Visual Studio you can run the following PowerShell script to get the latest version of HareDu...

PM> Install-Package HareDu

or if you want a specific version of HareDu you can do the following...

PM> Install-Package -Version <version> HareDu

Example,

PM> Install-Package -Version 2.0.0 HareDu

Since HareDu 2 was built primarily using Core APIs in Mono 5.x, it is now possible to get it in your preferred .NET environment on your preferred operating systems (e.g. Windows, macOS, Linux, etc.). 

You need the following packages if you are using; 

| API  | Packages Needed |
|---| --- |
| **Broker** | HareDu |
|  | HareDu.Core |
| **Snapshot** | HareDu.Snapshotting |
|  | HareDu |
|  | HareDu.Core |
| **Diagnostics** | HareDu.Diagnostics |
|  | HareDu.Snapshotting |
|  | HareDu |
|  | HareDu.Core |
| **Autofac** | HareDu.AutofacIntegration |
|  | HareDu.Snapshotting |
|  | HareDu.Diagnostics |
|  | HareDu |
|  | HareDu.Core |
| **.NET Core**  | HareDu.CoreIntegration |
|  | HareDu.Snapshotting |
|  | HareDu.Diagnostics |
|  | HareDu |
|  | HareDu.Core |


# Dependencies
.NET Framework 4.6.2 or above/.NET Core 2.1 or above

JSON.NET 12.0.2 or above

ASP.NET WebAPI 5.2.3 or above


# Debugging


If you find that making an API call is failing for reasons unknown, HareDu 2 introduces a way to return a text representation of the serialized JSON of the returned ```Result``` or ```Result<T>``` monads. Here is an example,

```string debugText = result.ToJsonString()```

That's it. So, the resulting output of calling the ToJsonString extension method might look something like this,

<pre><code class="json">
{
  &quot;timestamp&quot;: &quot;2018-12-31T18:04:39.511627+00:00&quot;,
  &quot;debugInfo&quot;: null,
  &quot;errors&quot;: [
    {
      &quot;reason&quot;: &quot;RabbitMQ server did not recognize the request due to malformed syntax.&quot;,
      &quot;timestamp&quot;: &quot;2018-12-31T18:04:39.511303+00:00&quot;
    }
  ],
  &quot;hasFaulted&quot;: true
}
</code></pre>



# Tested

macOS Sierra 10.15.2 (Catalina)

RabbitMQ 3.7.x, 3.8.2

Erlang OTP 22.0.4 (x64)

.NET 4.6.2 Framework

.NET Core 2.1


https://www.rabbitmq.com/monitoring.html

https://www.rabbitmq.com/heartbeats.html

https://www.rabbitmq.com/nettick.html

https://www.rabbitmq.com/management.html

https://www.rabbitmq.com/memory-use.html

https://pulse.mozilla.org/doc/stats.html

https://docs.appoptics.com/kb/host_infrastructure/integrations/rabbitmq/

https://www.datadoghq.com/blog/rabbitmq-monitoring/

https://www.rabbitmq.com/rabbitmqctl.8.html

https://www.rabbitmq.com/channels.html

https://www.rabbitmq.com/confirms.html

https://www.rabbitmq.com/heartbeats.html

https://www.rabbitmq.com/logging.html#connection-lifecycle-events



# Testing with RabbitMQ

Under the "IntegrationTesting" solution folder you will find two projects of note, HareDu.IntegrationTesting.Publisher and HareDu.IntegrationTesting.Consumer, respectively. These projects use the popular OSS project MassTransit to interact with the RabbitMQ broker for sending and receiving messages. Follow the below steps in order to test the Diagnostic API.

Ensure that your RabbitMQ broker has the proper plugins enabled by following the below documentation.
https://www.rabbitmq.com/management.html#clustering


If using .NET Core...  
1. Create a VirtualHost called "TestVirtualHost"
   Note: This can be done by either using the HareDu Broker API or by logging in to the RabbitMQ UI and creating a vhost
2. Bring up a command prompt (e.g., Terminal on MacOS) and execute the following command to start a consumer:
   dotnet ~/<your_path_here>/HareDu2/src/Consumer/bin/Debug/netcoreapp2.1/HareDu.IntegrationTesting.Consumer.dll
3. Once the consumer(s) have been started, bring up a command prompt (e.g., Terminal on MacOS) and execute the following command to start publishing messages:
    dotnet ~/<your_path_here>/HareDu2/src/Publisher/bin/Debug/netcoreapp2.1/HareDu.IntegrationTesting.Publisher.dll

Note: if you are using JetBrains Rider you can simply configure both projects and run them within the IDE.