# Autofac Integration

For those that are interested in quickly building applications with HareDu 2, the best way to do that is by using one of the supported IoC integration packages. In this section we will go over how to use the Autofac Integration package in your applications.


#### Registering API objects
The very first thing you need to do is register/initialize the appropriate objects you will need to perform operations on the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported IoC containers. Currently, HareDu 2 supports only two IoC containers; Autofac and .NET Core, respectively.

*Autofac*
```csharp
builder.RegisterModule<HareDuModule>();
```

The above steps represent the minimum required code to get something up and working without an IoC container. However, if you want to use IoC then its even easier. Since HareDu is a fluent API, you can method chain everything together like so...

*Autofac*
```csharp
var result = await container.Resolve<IBrokerObjectFactory>()
    .Object<Queue>()
    .GetAll();
```

### Snapshot API

The Snapshot API sits atop the Broker API and provides a high level rollup of RabbitMQ broker metrics. Each snapshot makes one or more calls to the Broker API methods aggregating the metric data into a developer-friendly object. Each snapshot is then captured on a timeline that can be then flushed to disk or saved to a database.

#### Registering API objects
The very first thing you need to do is register/initialize the appropriate objects you will need to take snapshots metric data on the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported IoC containers. Currently, HareDu 2 supports only two IoC containers; Autofac and .NET Core, respectively.

*Autofac*
```csharp
builder.RegisterModule<HareDuSnapshotModule>();
```

Note: The IoC container code that comes with HareDu currently defaults to file based configuration so you will need to make the appropriate changes to the haredu.yaml file.

<br>

*The above code becomes even simpler using an IoC container. Below is how you would take a snapshot on the first take...*

*Autofac*
```csharp
var lens = await container.Resolve<ISnapshotFactory>()
    .Lens<BrokerQueuesSnapshot>()
    .TakeSnapshot();
```

### Diagnostics API

The Diagnostics API sits atop the Snapshot API, providing a means to scan snapshot data for issues. This API consists of what we call *diagnostic probes* whose purpose is to analyze a particular set of data from a particular snapshot and returns a result of that analysis back to the calling *diagnostic scan*. A diagnostic scan looks at the entire snapshot and determines which probes should be called. The ```DiagnosticScanner``` is the single point of contact to analyzing snapshot data with the Diagnostic API.

#### Registering API objects
The very first thing you need to do is register/initialize the appropriate objects you will need to perform diagnostic scans on snapshot data captured from the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported IoC containers. Currently, HareDu 2 supports only two IoC containers; Autofac and .NET Core, respectively.

*Autofac*
```csharp
builder.RegisterModule<HareDuDiagnosticsModule>();
```

Note: The IoC container code that comes with HareDu currently defaults to file based configuration so you will need to make the appropriate changes to the haredu.yaml file.

<br>

