HareDu 2
========
HareDu 2 is a .NET client and library that consumes the RabbitMQ REST API and is used to manage and monitor a RabbitMQ server or cluster.


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

		var yourResourceFactory = client.Factory<YourResourcesInterface>();

Example,

    var resource = client.Factory<VirtualHost>();

Calling the Factory method is essential to accessing HareDu resources as it is responsible for setting up the client for which requests for resources are sent. You must pass an interface that inherits from HareDu.Resources.ResourceClient. 


3.) Call your properties and methods that are available to you based on your resource factory you setup in step 2.

        Result result = await resourceFactory.Factory<Resource>()
                                             .Create(<exchange>, <vhost>,
                                                       x =>
                                                           {
                                                               x.IsDurable();
                                                               x.UsingRoutingType(r => r.Fanout());
                                                           });

Example,

        Result result = await resource.Factory<Exchange>()
                                      .Create(<exchange>, <vhost>,
                                            x =>
                                                {
                                                   x.IsDurable();
                                                   x.UsingRoutingType(r => r.Fanout());
                                                });


Developers have the option of either calling the API in steps or all at once via HareDu's fluent methods like so,

HareDuClient client = HareDuFactory.New(x =>
								            {
								                x.ConnectTo("http://localhost:15672");
								                x.EnableLogging(s => s.Logger("haredu_logger"));
                                                x.UsingCredentials("guest", "guest");
								            });

Result result = await client
    .Factory<Exchange>()
    .Create("TestExchange", "HareDuVhost", x =>
									    {
									        x.IsDurable();
									        x.UsingRoutingType(r => r.Fanout());
									    });

...or

Result result = await HareDuFactory
	.New(x =>
            {
                x.ConnectTo("http://localhost:15672");
                x.EnableLogging(s => s.Logger("haredu_logger"));
                x.UsingCredentials("guest", "guest");
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

2.) You know the URL and port to access the RabbitMQ REST API you want to interact with

3.) You have some valid credentials to communite with the RabbitMQ server (default credentials are: username => guest, password => guest)


Dependencies
============
.NET Framework 4.5.2

JSON.NET

ASP.NET WebAPI

Common.Logging


Tested
======
macOS Sierra 10.12.5
Windows Server 2008 R2

RabbitMQ 3.6.9

Erlang OTP R19.3 (x64)

.NET 4.5.2 Framework

