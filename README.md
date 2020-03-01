# HareDu 2

![Join the chat at https://gitter.im/HareDu2/Lobby](https://img.shields.io/gitter/room/haredu2/HareDu2?style=flat)
![NuGet downloads](https://img.shields.io/nuget/dt/haredu?style=flat)

HareDu is a .NET library for managing and monitoring RabbitMQ clusters using the RabbitMQ RESTful API.

HareDu is Apache 2.0 licensed.

Docs under construction here

https://ahives.gitbooks.io/haredu2/content/

### HareDu 2 NuGet Packages

| Package Name | Framework | .NET Standard |
|---| --- | --- |
| **Main** |  |  |
| HareDu.Core | 4.6.2 | 2.0 |
| **API** |  |  |
| HareDu | 4.6.2 | 2.0 |
| HareDu.Snapshotting | 4.6.2 | 2.0 |
| HareDu.Diagnostics | 4.6.2 | 2.0 |
| **Containers** | | |
| HareDu.AutofacIntegration | 4.6.2 | 2.0 |
| HareDu.CoreIntegration | 4.6.2 | 2.0 |


# Why HareDu 2?

If you are familiar with HareDu, you should know that HareDu 2 introduces some really cool new functionality. HareDu 2 came about from feedback of production deployments and because the original API was lacking in some key areas. In particular, HareDu 2 introduces the following enhancements:
1. Increased test coverage
2. Improved low level administrative APIs
3. New APIs for diagnostics and snapshotting broker information
4. Dependency Injection (e.g., Autofac, .NET Core) support for quick API registration
5. .NET Core support 


# Fundamentals
HareDu 2 comes with three major APIs; that is, Broker, Snapshot, and Diagnostics, respectively. To use HareDu, you must have the appropriate RabbitMQ plugins installed and enabled.

### Configuration
Configuring your HareDu-powered application can be as simple as modifying the *haredu.yaml* file. At present, there are two sections in the YAML file to consider, that is, *broker* and *diagnostics*, respectively. The section called *broker* encompasses the configuration needed to use the Broker API. The section called *diagnostics* has the configuration needed to use the Diagnostics API.

| Setting | Description |
| ---| --- |
| url | The url of the RabbitMQ node that has metrics enabled. |
| username | This user should have administrative access to the RabbitMQ broker. |
| password | Decrypted password of a user that has administrative access to the RabbitMQ broker. |
| high-closure-rate-warning-threshold | Defines the maximum acceptable rate of which connections are closed on the RabbitMQ broker to determine whether or not it is considered healthy. If the rate of which connections are closed is greater than or equal to this setting, a warning is generated, which implies that the application communicating with the broker may be experiencing issues. Otherwise, if the rate of closed connections is less than this setting then the system is considered to be operating normally. |
| high-creation-rate-warning-threshold | Defines the maximum acceptable rate of which connections to the RabbitMQ broker can be made in order to determine whether or not it is considered healthy. If the rate of which connections are created is greater than or equal to this setting, a warning is generated, which implies that the application communicating with the broker may be experiencing issues. Otherwise, if the rate of created connections is less than this setting then the system is consider to be operating normally. |
| queue-high-flow-threshold | Defines the maximum acceptable number of messages that can be published to a queue. If the number of published messages is greater than or equal to this setting then this queue is considered unhealthy  |
| queue-low-flow-threshold |  |
| message-redelivery-coefficient |  |
| socket-usage-coefficient |  |
| runtime-process-usage-coefficient |  |
| file-descriptor-usage-warning-coefficient |  |
| consumer-utilization-warning-coefficient |  |

HareDu YAML looks like this...
```yaml
---
  broker:
      url:  http://localhost:15672
      username: guest
      password: guest
  diagnostics:
    high-closure-rate-warning-threshold:  100
    high-creation-rate-warning-threshold: 100
    queue-high-flow-threshold:  100
    queue-low-flow-threshold: 20
    message-redelivery-coefficient: 0.50
    socket-usage-coefficient: 0.60
    runtime-process-usage-coefficient:  0.65
    file-descriptor-usage-warning-coefficient:  0.65
    consumer-utilization-warning-coefficient: 0.50
...
```
There are several ways to configure HareDu. Let's look at the major scenarios.

#### I just want to configure the Broker/Snapshot API
You can either do this explicitly by calling the ```BrokerConfigProvider``` directly like so...
```csharp
var provider = new BrokerConfigProvider();
var config = provider.Configure(x =>
{
    x.ConnectTo("http://localhost:15672");
    x.UsingCredentials("guest", "guest");
});
```
...or you can simply read YAML configuration. There are two ways to read YAML. Either you can read a YAML configuration file like so...
```csharp
var validator = new YourCustomConfigValidator();
var provider = new YamlFileConfigProvider(validator);

provider.TryGet("haredu.yaml", out HareDuConfig config);
```
...or you can read YAML text like this...
```csharp
var validator = new YourCustomConfigValidator();
var provider = new YamlConfigProvider(validator);

provider.TryGet(yamlText, out HareDuConfig config);
```
From here you need only call ```config.Broker``` to access the broker configuration. In the above example, ```YamlFileConfigProvider``` and ```YamlConfigProvider``` are shown to be initialized by explicitly passing ```IConfigValidator```, which can be a custom validator. However, if you want to use the default validator then use the parameterless constructor.
<br>

#### I just want to configure the Diagnostics API
There are a couple ways to configure the Diagnostics API. Since most of the default diagnostic probes are configurable by passing in settings, we give you a way to codify those settings. The first option is to read the *haredu.yaml* file. The other option is set the configuration explicitly like so...

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
```

<br>

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

*Autofac*
```csharp
builder.RegisterModule<HareDuModule>();
```

*.NET Core DI*
```csharp
var services = new ServiceCollection()
    .AddHareDu()
    .BuildServiceProvider();
```

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
var result = await factory
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

*Autofac*
```csharp
builder.RegisterModule<HareDuSnapshotModule>();
```

*.NET Core DI*
```csharp
var services = new ServiceCollection()
    .AddHareDuSnapshot()
    .BuildServiceProvider();
```

Note: The IoC container code that comes with HareDu currently defaults to file based configuration so you will need to make the appropriate changes to the haredu.yaml file.

<br>

#### Taking snapshots
Once you have registered a SnapshotFactory, it is easy to take a snapshot.

**Step 1: Define what type of snapshot you want to take**
```csharp
var factory = new SnapshotFactory(new BrokerObjectFactory(config));
var lens = factory.Lens<BrokerQueues>();
```
In this code snippet, the lens variable returns a ``SnapshotLens`` for taking snapshots of type ```BrokerQueues```.

**Step 2: Take the snapshot**
```csharp
lens.TakeSnapshot();
```

*The above code becomes even simpler using an IoC container. Below is how you would take a snapshot on the first take...*

*Autofac*
```csharp
var lens = await container.Resolve<ISnapshotFactory>()
    .Lens<BrokerQueues>()
    .TakeSnapshot();
```

*.NET Core DI*
```csharp
var lens = await services.GetService<ISnapshotFactory>()
    .Lens<BrokerQueues>()
    .TakeSnapshot();
```
<br>

#### Viewing snapshots

Snapshots are accessible via the Timeline property on the ```SnapshotFactory```. Getting the most recent snapshot is as easy as calling the ```MostRecent``` extension method off of the ```History``` property on the lens like so...  

```csharp
var result = factory.History.MostRecent();
```
Since ```MostRecent``` returns a ```SnapshotResult<T>``` you can easily get the actual snapshot data by simple calling the ```Snapshot``` property like this...

```csharp
var snapshot = result.Snapshot;
```
...or more concisely...

```csharp
var snapshot = factory.History.MostRecent().Snapshot;
```

#### Registering Observers

When setting up ```SnapshotFactory```, you can register observers. These observers should implement ```IObserver<T>``` where ```T``` is ```SnapshotResult<TSnapshot>```. Each time a snapshot is taken (i.e. when the ```Execute``` method is called), all registered observers will be notified with an object of ```SnapshotResult<TSnapshot>```. Registering an observer is easy enough (see code snippet below) but be sure to do so before calling the ```Execute``` method or else the registered observers will not receive notifications.

```csharp
var lens = factor
    .Lens<BrokerQueues>()
    .RegisterObserver(new SomeCoolObserver())
    .TakeSnapshot();
```

#### Putting it altogether
Below is an example of taking a snapshot of RabbitMQ broker queue data, registering an observer to receive notifications when said snapshot is taken, and returning the most recent snapshot from the snapshot timeline.
```csharp
// Define the snapshot observer
public class BrokerQueuesObserver :
    IObserver<SnapshotContext<BrokerQueuesSnapshot>>
{
    public void OnCompleted() => throw new NotImplementedException();

    public void OnError(Exception error) => throw new NotImplementedException();

    public void OnNext(SnapshotContext<BrokerQueuesSnapshot> value) => throw new NotImplementedException();
}

var provider = new YamlFileConfigProvider();

// Get the API configuration
provider.TryGet("haredu.yaml", out HareDuConfig config);

// Initialize the snapshot factory
var factory = new SnapshotFactory(config.Broker);

// Select a snapshot lens and optionally register observers on the lens
var lens = factory
    .Lens<BrokerQueues>()
    .RegisterObserver(new BrokerQueuesObserver());

// Take a snapshot
lens.TakeSnapshot();

var result = lens.History.MostRecent();
```

### Diagnostics API

The Diagnostics API sits atop the Snapshot API, providing a means to scan snapshot data for issues. This API consists of what we call *diagnostic probes* whose purpose is to analyze a particular set of data from a particular snapshot and returns a result of that analysis back to the calling *diagnostic scan*. A diagnostic scan looks at the entire snapshot and determines which probes should be called. The ```DiagnosticScanner``` is the single point of contact to analyzing snapshot data with the Diagnostic API.

#### Registering API objects
The very first thing you need to do is register/initialize the appropriate objects you will need to perform diagnostic scans on snapshot data captured from the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported IoC containers. Currently, HareDu 2 supports only two IoC containers; Autofac and .NET Core, respectively.

*Autofac*
```csharp
builder.RegisterModule<HareDuDiagnosticsModule>();
```

*.NET Core DI*
```csharp
var services = new ServiceCollection()
    .AddHareDuDiagnostics()
    .BuildServiceProvider();
```

Note: The IoC container code that comes with HareDu currently defaults to file based configuration so you will need to make the appropriate changes to the haredu.yaml file.

<br>

Registering objects without IoC containers is pretty simple as well...

```csharp
var scanner = new DiagnosticScanner(config.Diagnostics, new KnowledgeBaseProvider());
```
Since the ```DiagnosticScanner``` should only be initialized once in your application, therefore, you should use the Singleton pattern. Please note that the IoC integrations registers ```DiagnosticScanner``` as a singleton. This applies to most things in HareDu 2.

#### Scanning snapshots

```csharp
var result = scanner.Scan(snapshot);
```

The above code will return a ```ScannerResult``` object, which contains the result of executing each diagnostic probe against the snapshot data.

#### Registering Observers

When setting up ```DiagnosticScanner```, you can register observers. These observers should implement ```IObserver<T>``` where ```T``` is ```SnapshotContext<TSnapshot>```. Each time a snapshot is taken (i.e. when the ```Scan``` method is called), all registered observers will be notified with an object of ```SnapshotContext<TSnapshot>```. Registering an observer is easy enough (see code snippet below) but be sure to do so before calling the ```Scan``` method or else the registered observers will not receive notifications.

```csharp
var result = scanner
    .RegisterObserver(new SomeCoolObserver())
    .Scan(snapshot);
```
<br>

#### Putting it altogether

Below is an example of scanning a ```BrokerQueues``` snapshot.

```csharp
// Define the scanner observer
public class BrokerQueuesScannerObserver :
    IObserver<DiagnosticProbeContext>
{
    public void OnCompleted() => throw new NotImplementedException();

    public void OnError(Exception error) => throw new NotImplementedException();

    public void OnNext(DiagnosticProbeContext value) => throw new NotImplementedException();
}

var provider = new YamlFileConfigProvider();

// Get the API configuration
provider.TryGet("haredu.yaml", out HareDuConfig config);

var factory = new SnapshotFactory(config.Broker);

// Take a snapshot
var lens = factory
    .Lens<BrokerQueues>()
    .TakeSnapshot();

// Initialize the diagnostic scanner and register the observer
var scanner = new DiagnosticScanner(config.Diagnostics, new KnowledgeBaseProvider())
    .RegisterObserver(new BrokerQueuesScannerObserver());

// Scan the results of the most recent snapshot taken
var result = scanner.Scan(lens.History.MostRecent());
```

### Gotchas

Registering an API will register all dependent objects so there is no need to include them. So, if you intend on using both the Broker and Snapshot APIs then including the Snapshot API will also register everything in the Broker API. The same can be said for the Diagnostic API, since it is dependent on both the Broker and Snapshot APIs.


## Get It
From the Package Manager Console in Visual Studio you can run the following PowerShell script to get the latest version of HareDu...

PM> Install-Package HareDu

or if you want a specific version of HareDu you can do the following...

PM> Install-Package -Version <version> HareDu

Example,

PM> Install-Package -Version 2.0.0 HareDu

The above applies for any NuGet package you wish to install.

## Using HareDu with RabbitMQ

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

# Dependencies
.NET Framework 4.6.2 or above/.NET Core 2.1 or above

JSON.NET 12.0.2 or above

ASP.NET WebAPI 5.2.3 or above


# Debugging

If you find that making an API call is failing for reasons unknown, HareDu 2 introduces a way to return a text representation of the serialized JSON of the returned ```Result``` or ```Result<T>``` monads. Here is an example,

```csharp
string debugText = result.ToJsonString();
```

That's it. So, the resulting output of calling the ToJsonString extension method might look something like this,

```json
{
  "timestamp": "2018-12-31T18:04:39.511627+00:00",
  "debugInfo": null,
  "errors": [
    {
      "reason": "RabbitMQ server did not recognize the request due to malformed syntax.",
      "timestamp": "2018-12-31T18:04:39.511303+00:00"
    }
  ],
  "hasFaulted": true
}
```

# Tested

|   | Version |
|---| --- |
| Operating System | macOS Catalina 10.15.3 |
| RabbitMQ | 3.7.x, 3.8.2 |
| Erlang OTP | 22.0.4 (x64) |
| .NET Runtime | Framework 4.6.2, Core 2.1 |

