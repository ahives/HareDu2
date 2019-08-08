HareDu 2
========

[![Join the chat at https://gitter.im/HareDu2/Lobby](https://badges.gitter.im/HareDu2/Lobby.svg)](https://gitter.im/HareDu2/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

HareDu 2 is a .NET client and library that consumes the RabbitMQ REST API and can be used to manage and monitor a RabbitMQ server or cluster.


Docs under construction here

https://ahives.gitbooks.io/haredu2/content/


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
.NET Framework 4.5.2 or above

.NET Core 2.1 or above

JSON.NET 11.0.2 or above

ASP.NET WebAPI 5.2.3 or above


Debugging
=========

If you find that making an API call is failing for reasons unknown, HareDu 2 introduces a way to return a text representation of the serialized JSON of the returned ```Result<T>``` monad. Here is an example,

<pre><code class="c#">Result result = await Client
    .Factory&lt;Shovel&lt;AMQP091Source, AMQP091Destination&gt;&gt;()
    .Shovel(x =&gt;
    {
        x.Configure(c =&gt;
        {
            c.Name(&quot;my-shovel&quot;);
            c.VirtualHost(&quot;%2f&quot;);
        });
        x.Source(s =&gt;
        {
            s.Uri(u =&gt; { u.Builder(b =&gt; { b.SetHeartbeat(1); }); });
            s.PrefetchCount(2);
            s.Queue(&quot;my-queue&quot;);
        });
        x.Destination(d =&gt;
        {
            d.Queue(&quot;another-queue&quot;);
            d.Uri(u =&gt;
            {
                u.Builder(b =&gt;
                {
                    b.SetHost(&quot;remote-server&quot;);
                });
            });
        });
    });

Console.WriteLine(result.ToJson());
</code></pre>

So, the output calling result.ToJson would be,

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









Tested
======
macOS Sierra 10.12.5/High Sierra/Mojave

Windows Server 2008 R2

RabbitMQ 3.6.9

Erlang OTP R19.3 (x64)

.NET 4.5.2 Framework

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



# Using API

https://www.rabbitmq.com/management.html#clustering-subset-of-nodes

Ensure that you have the following configuration values set to enable the API to pull statistics from your RabbitMQ cluster:

Configuration value "vm_memory_calculation_strategy" is set to "rss"


Testing with RabbitMQ
=====================
Under the "simulation" solution folder you will find two projects of note, Publisher and Consumer, respectively. These projects use the popular OSS project MassTransit to interact with the RabbitMQ broker for sending and receiving messages. Follow the below steps in order to test the Diagnostic API.

If using .NET Core...  
1. Create a VirtualHost called "TestVirtualHost"
   Note: This can be done by either using the low level HareDu API or by logging in to the RabbitMQ UI and creating a vhost
2. Bring up a command prompt (e.g., Terminal on MacOS) and execute the following command to start a consumer:
   
   dotnet ~/<your_path_here>/HareDu2/src/Consumer/bin/Debug/netcoreapp2.1/Consumer.dll
   
3. Once the consumer(s) have been started, bring up a command prompt (e.g., Terminal on MacOS) and execute the following command to start publishing messages:

    dotnet ~/<your_path_here>/HareDu2/src/Publisher/bin/Debug/netcoreapp2.1/Publisher.dll

