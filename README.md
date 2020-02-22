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
6. 

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

#### Registering the Broker API

*with Autofac*

```builder.RegisterModule<HareDuModule>();```

*with .NET Core DI*

```services.AddHareDu();```

Note: The IoC container code that comes with HareDu currently defaults to file based configuration so you will need to make the appropriate changes to the haredu.yaml file.

<br>

Without IoC containers you would write the following code...

*with YAML configuration*
<pre><code class="c#">
var provider = new YamlConfigProvider();

provider.TryGet("haredu.yaml", out HareDuConfig config);

var factory = new BrokerObjectFactory(config.Broker);
</code></pre>

*without YAML configuration...*

<pre><code class="c#">
var provider = new BrokerConfigProvider();
var config = provider.Configure(x =>
{
    x.ConnectTo("http://localhost:15672");
    x.UsingCredentials("guest", "guest");
});

var factory = new BrokerObjectFactory(config);
</code></pre>


#### Using the Broker API
The Broker API is considered the low level API because it allows you to administer RabbitMQ (e.g., users, queues, exchanges, etc.).

**Step 1: Get a broker object**

```var obj = factory.Object<Exchange>();```


**Step 2: Call methods from API object**

```var result = obj.GetAll();```

Note: The above code will return a `Task<T>` so if you want to return the unwrapped ```Result``` or ```Result<T>``` monad you need to use an ```await``` or call the HareDu ```Unfold``` extension method.

Using the async/await pattern...

```var result = await obj.GetAll();```

Using the HareDu extension method...

```var result = obj.GetAll().Unfold();```

The above steps represent the minimum required code to get something up and working without an IoC container. However, if you want to use IoC then its even easier.

From this point you can skip to step 3. Since HareDu is a fluent API, you can method chain steps 3 and 4 together like so...

**Autofac**

```var result = await container.Resolve<IBrokerObjectFactory>().Object<Exchange>().GetAll();```

**.NET Core**

```var result = await services.GetService<IBrokerObjectFactory>().Object<Exchange>().GetAll();```


### Snapshot API

The Snapshotting API sits atop the Broker API and provides a high level rollup of RabbitMQ broker metrics. Each snapshot makes one or more calls to the Broker API methods aggregating the metric data into a developer-friendly object. Each snapshot is then captured on a timeline that can be then flushed to disk or saved to a database.

#### Registering the Snapshot API

*with Autofac*

```builder.RegisterModule<HareDuSnapshotModule>();```

*with .NET Core DI*

```services.AddHareDuSnapshot();```

Note: The IoC container code that comes with HareDu currently defaults to file based configuration so you will need to make the appropriate changes to the haredu.yaml file.

<br>

Without IoC containers you would write the following code...

**Option 1: with YAML configuration**
<pre><code class="c#">
var provider = new YamlConfigProvider();

provider.TryGet("haredu.yaml", out HareDuConfig config);

var brokerFactory = new BrokerObjectFactory(config.Broker);
var factory = new SnapshotFactory(brokerFactory);
</code></pre>

**Option 2a: without YAML configuration**

<pre><code class="c#">
var provider = new BrokerConfigProvider();
var config = provider.Configure(x =>
{
    x.ConnectTo("http://localhost:15672");
    x.UsingCredentials("guest", "guest");
});

var brokerFactory = new BrokerObjectFactory(config);
var factory = new SnapshotFactory(brokerFactory);
</code></pre>

**Option 2b: without YAML configuration**

<pre><code class="c#">
var provider = new BrokerConfigProvider();
var config = provider.Configure(x =>
{
    x.ConnectTo("http://localhost:15672");
    x.UsingCredentials("guest", "guest");
});

var factory = new SnapshotFactory(config);
</code></pre>

<br>

#### Taking snapshots
Once you have registered a SnapshotFactory, it is easy to take a snapshot.

**Step 1: Define which snapshot you want to take**

```var snapshot = factory.Snapshot<BrokerQueues>();```

**Step 2: Take the snapshot**

```snapshot.Execute();```

<br>

*The above code becomes even simpler using IoC. Below is how you would take a snapshot on the first take...*

*with Autofac*

```var result = await container.Resolve<ISnapshotFactory>().Snapshot<BrokerQueues>().Execute();```

*with .NET Core DI*

```var result = await services.GetService<ISnapshotFactory>().Snapshot<BrokerQueues>().Execute();```


#### Viewing snapshots

Snapshots are accessible via the ```Timeline``` property on the SnapshotFactory. Getting the most recent snapshot is as easy as calling the MostRecent extension method like so...  
```var factory.Timeline.MostRecent()```



### Diagnostics API

blah, blah, blah


### Gotchas

Registering an API will register all dependent objects so there is no need to include them. So, if you intend on using both the Broker and Snapshot APIs then including the Snapshot API will also register everything in the Broker API. The same can be said for the Diagnostic API, since it is dependent on both the Broker and Snapshot APIs.


# Get It
You can now get HareDu 2 on NuGet by searching for HareDu. Also, you can check out HareDu at https://github.com/ahives/HareDu2

From the Package Manager Console in Visual Studio you can run the following PowerShell script to get the latest version of HareDu...

PM> Install-Package HareDu

or if you want a specific version of HareDu you can get your Du by doing...

PM> Install-Package -Version <version> HareDu

Example,

PM> Install-Package -Version 2.0.0 HareDu

Since HareDu 2 was built primarily using Core APIs in Mono 5.x, it is now possible to get it in your preferred .NET environment on your preferred operating systems (e.g. Windows, macOS, Linux, etc.). 


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




# Using API

https://www.rabbitmq.com/management.html#clustering-subset-of-nodes


# Testing with RabbitMQ

Under the "simulation" solution folder you will find two projects of note, Publisher and Consumer, respectively. These projects use the popular OSS project MassTransit to interact with the RabbitMQ broker for sending and receiving messages. Follow the below steps in order to test the Diagnostic API.

If using .NET Core...  
1. Create a VirtualHost called "TestVirtualHost"
   Note: This can be done by either using the low level HareDu API or by logging in to the RabbitMQ UI and creating a vhost
2. Bring up a command prompt (e.g., Terminal on MacOS) and execute the following command to start a consumer:
   
   dotnet ~/<your_path_here>/HareDu2/src/Consumer/bin/Debug/netcoreapp2.1/Consumer.dll
   
3. Once the consumer(s) have been started, bring up a command prompt (e.g., Terminal on MacOS) and execute the following command to start publishing messages:

    dotnet ~/<your_path_here>/HareDu2/src/Publisher/bin/Debug/netcoreapp2.1/Publisher.dll

