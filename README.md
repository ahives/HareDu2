HareDu 2
========

[![Join the chat at https://gitter.im/HareDu2/Lobby](https://badges.gitter.im/HareDu2/Lobby.svg)](https://gitter.im/HareDu2/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
HareDu 2 is a .NET client and library that consumes the RabbitMQ REST API and can be used to manage and monitor a RabbitMQ server or cluster.


There was a lot of rethought that went in to HareDu 2, which is why the decision was made to introduce breaking changes for those using HareDu 1.x. This decision was not taken likely. Evaluating on whether to continue building on top of HareDu 1.x came down to the following things:

1. Support for non-Windows machines
2. Strategic evolution of the API
3. Adoption
4. Rise of cloud computing

Let's look at each of these and compare to what HareDu 1.x offered. Although HareDu 1.x that it was a very compelling API for managing RabbitMQ, however, we felt that it was missing support for non-Windows operating systems. This is a little ironic considering RabbitMQ itself runs on multiple operating systems. Back when HareDu 1.0 shipped in 2013, there was two camps for .NET, on one end of the spectrum was its inventor, Microsoft, and at the other end was Xamarin's Mono. Although great efforts were made by Xamarin to ensure API parity and binary compatibility with the .NET Framework, the fact of the matter is that it was a bit challenging for indepdent application developers to target both runtimes. However, fast forward 4 years, a lot has changed; Microsot now owns Xamarin and Mono and together have recently introduced .NET Standard and .NET Core. In addition, there has been a lot of evolution in tooling around .NET on non-Windows machines such as JetBrains' latest IDE, Rider. This perfect storm of tooling and a melding of minds of sorts around the future of the .NET was one of the primary factors that led to the decision to rethink HareDu and target non-Windows operating systems as well.

That said, we also looked at better ways to strategically evolve the API with the intent to disturb developers as little as possible. Just like everything else, HareDu had to evolve or die. When talking about the evolution of the API there are a couple things that come to mind:
1. Functionality that did not make sense was deprecated. A good example of this would be how resources have multiple Get methods. Instead following this tradition, we decided to go with a single method and introduce extensions that hang off of the result set to do the necessary filtering. The thought behind such decisions were to remove clutter in the API, in this case returning all the data and allowing the developer to make decisions around how to constrain the result.
2. All methods return a common immutable object. This allows the API to evolve with as little impedance as possible to developers. It also increases accuracy of how the API is used since developers cannot accidentally change the returned object in their applications.


If you are familiar with HareDu 1.x the following code

           var data = HareDuFactory.New(x =>
                {
                    x.ConnectTo(Settings.Default.HostUrl);
                    x.EnableLogging(y => y.Logger(Settings.Default.LoggerName));
                })
                .Factory<VirtualHostResources>(x => x.Credentials(Settings.Default.LoginUsername, Settings.Default.LoginPassword))
                .Queue
                .GetAll(x => x.VirtualHost(Settings.Default.VirtualHost))
                .Data();





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



Assumptions
===========
1.) You have RabbitMQ running in some environment that is reachable from the machine that you are running HareDu applications on

2.) You know the URL and port (ex: localhost:15672) to access the RabbitMQ REST API you want to interact with

3.) You have valid user credentials to communicate with the RabbitMQ server (default credentials are: username => guest, password => guest)


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


Using Extensions
================

Now in HareDu 2 there are extensions to help you with dealing with the returned objects of the various API methods. In HareDu 1.x, 


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

