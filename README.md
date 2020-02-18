HareDu 2
========

![Join the chat at https://gitter.im/HareDu2/Lobby](https://img.shields.io/gitter/room/haredu2/HareDu2?style=flat)
![NuGet downloads](https://img.shields.io/nuget/dt/haredu?style=flat)

.NET API for managing and monitoring RabbitMQ clusters using the RabbitMQ RESTful API.


Docs under construction here

https://ahives.gitbooks.io/haredu2/content/

History
=======
HareDu 2 is a complete rewrite of the original HareDu 1.x API. This rewrite came about from feedback of production deployments and because the original API was lacking in some key areas. In particular,  
1. Increased test coverage
2. Improved low level administrative APIs
3. New APIs for diagnostics, cluster snapshotting and monitoring
4. Dependency Injection (e.g., Autofac, .NET Core) support for quick API registration
5. .NET Core support


Get It
======

You can now get HareDu on NuGet by searching for HareDu. Also, you can check out HareDu at http://www.nuget.org/packages/HareDu/

From the Package Manager Console in Visual Studio you can run the following PowerShell script to get the latest version of HareDu...

PM> Install-Package HareDu

or if you want a specific version of HareDu you can get your Du by doing...

PM> Install-Package -Version <version> HareDu

Example,

PM> Install-Package -Version 2.0.0 HareDu

Since HareDu 2 was built primarily using Core APIs in Mono 5.x, it is now possible to get it in your preferred .NET environment on your preferred operating systems (e.g. Windows, macOS, Linux, etc.). 


Dependencies
============
.NET Framework 4.6.2 or above

.NET Core 2.1 or above

JSON.NET 12.0.2 or above

ASP.NET WebAPI 5.2.3 or above


# Debugging


If you find that making an API call is failing for reasons unknown, HareDu 2 introduces a way to return a text representation of the serialized JSON of the returned ```Result<T>``` monad. Here is an example,

<pre><code class="c#">
var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create(x =>
                {
                    x.Queue("TestQueue31");
                    x.Configure(c =>
                    {
                        c.IsDurable();
                        c.HasArguments(arg => { arg.SetQueueExpiration(1000); });
                    });
                    x.Target(t => t.VirtualHost("HareDu"));
                });
            
string debugText = result.ToJsonString());
</code></pre>


So, the output calling the ToJsonString extension method would look something like this,

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

RabbitMQ 3.7.15

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

Ensure that you have the following configuration values set to enable the API to pull statistics from your RabbitMQ cluster:

Configuration value "vm_memory_calculation_strategy" is set to "rss"


# Testing with RabbitMQ

Under the "simulation" solution folder you will find two projects of note, Publisher and Consumer, respectively. These projects use the popular OSS project MassTransit to interact with the RabbitMQ broker for sending and receiving messages. Follow the below steps in order to test the Diagnostic API.

If using .NET Core...  
1. Create a VirtualHost called "TestVirtualHost"
   Note: This can be done by either using the low level HareDu API or by logging in to the RabbitMQ UI and creating a vhost
2. Bring up a command prompt (e.g., Terminal on MacOS) and execute the following command to start a consumer:
   
   dotnet ~/<your_path_here>/HareDu2/src/Consumer/bin/Debug/netcoreapp2.1/Consumer.dll
   
3. Once the consumer(s) have been started, bring up a command prompt (e.g., Terminal on MacOS) and execute the following command to start publishing messages:

    dotnet ~/<your_path_here>/HareDu2/src/Publisher/bin/Debug/netcoreapp2.1/Publisher.dll


# Coming Soon - HareDu 2

HareDu 2 will introduce some pretty awesome new APIs and features but unfortunately there will be breaking changes to those who are currently using HareDu 1.x.

**New Features**
1. API registration using popular Dependency Injection containers (e.g., Autofac and .NET Core to start)
2. Brand new APIs (e.g., taking snapshots, running diagnostic scans, etc.)
3. File-based API configuration using YAML
