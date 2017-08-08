HareDu 2
========
HareDu 2 is a .NET client and library that consumes the RabbitMQ REST API and can be used to manage and monitor a RabbitMQ server or cluster.


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


Getting Started
===============

1.) Setup your HareDu client by calling the ConnectTo method and passing in the URL (http://<IP_address>:<port>) to the RabbitMQ server

		var client = HareDuFactory.New(x => {
		    x.ConnectTo("http://<IP_address>:<port>");
		    x.UsingCredentials("<username>", "<password>");
		});

The above represents the minimm configuration of the client but HareDu also exposes the EnableLogging method for logging and the TimeoutAfter method for setting the timeout threshold.

***Please note that the default RabbitMQ port is 15672


2.) Setup a resource factory using your user credentials

     var yourResourceFactory = client.Factory<YourResourceInterface>();

Example,

     var resource = client.Factory<VirtualHost>();

Calling the Factory method is essential to accessing HareDu resources as it is responsible for setting up the client for which requests for resources are sent. You must pass an interface that inherits from HareDu.Resources.ResourceClient. 


3.) Call your properties and methods that are available to you based on your resource factory you setup in step 2.

            Result result = await client
                .Factory<Exchange>()
                .Create(<exchange>, <vhost>, x =>
                {
                    x.IsDurable();
                    x.UsingRoutingType(r => r.Fanout());
                });

Example,

            Result result = await client
                .Factory<Exchange>()
                .Create("TestExchange", "HareDuVhost", x =>
                {
                    x.IsDurable();
                    x.UsingRoutingType(r => r.Fanout());
                });


Developers have the option of either calling the API in steps or all at once via HareDu's fluent methods like so,

            HareDuClient client = HareDuFactory.Create(x =>
            {
                x.ConnectTo("http://localhost:15672");
                x.Logging(s =>
                {
                    s.Enable();
                    s.UseLogger("HareDuLogger");
                });
                x.UsingCredentials("guest", "guest");
                x.TransientRetry(s =>
                {
                    s.Enable();
                    s.RetryLimit(3);
                });
            });

            Result result = await client
                .Factory<Exchange>()
                .Create("TestExchange", "HareDuVhost", x =>
                {
                    x.IsDurable();
                    x.UsingRoutingType(r => r.Fanout());
                });

...or

            Result result = await HareDuFactory.Create(x =>
                {
                    x.ConnectTo("http://localhost:15672");
                    x.Logging(s =>
                    {
                        s.Enable();
                        s.UseLogger("HareDuLogger");
                    });
                    x.UsingCredentials("guest", "guest");
                    x.TransientRetry(s =>
                    {
                        s.Enable();
                        s.RetryLimit(3);
                    });
                })
                .Factory<Exchange>()
                .Create("TestExchange", "HareDuVhost", x =>
                {
                    x.IsDurable();
                    x.UsingRoutingType(r => r.Fanout());
                });


Assumptions
===========
1.) You have RabbitMQ running in some environment that is reachable from the machine that you are running HareDu applications on

2.) You know the URL and port (ex: localhost:15672) to access the RabbitMQ REST API you want to interact with

3.) You have valid user credentials to communicate with the RabbitMQ server (default credentials are: username => guest, password => guest)


Dependencies
============
.NET Framework 4.5.2 or above

JSON.NET 10.0.3 or above

ASP.NET WebAPI 5.2.3 or above

Common.Logging 3.3.1

Polly 5.3.0

Log4Net 2.0.8


Tested
======
macOS Sierra 10.12.5

Windows Server 2008 R2

RabbitMQ 3.6.9

Erlang OTP R19.3 (x64)

.NET 4.5.2 Framework

