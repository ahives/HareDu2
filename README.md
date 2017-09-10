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

