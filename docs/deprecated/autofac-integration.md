# Autofac Integration

For those that are interested in quickly building applications with HareDu 2, the best way to do that is by using one of the supported IoC integration packages. In this section we will go over how to use the Autofac Integration package in your applications.

Before you can use any of the APIs, you must configure HareDu by calling ```AddHareDuConfiguration``` like so...
```csharp
var container = new ContainerBuilder()
                .AddHareDuConfiguration($"{Directory.GetCurrentDirectory()}/my_config.yaml")
                .Build();
```
<br>


#### Broker API
Since the Broker API is the lowest level API, you need only call the ```AddHareDu``` extension method off of ```ContainerBuilder``` like so...

```csharp
var container = new ContainerBuilder()
                .AddHareDuConfiguration($"{Directory.GetCurrentDirectory()}/my_config.yaml")
                .AddHareDu()
                .Build();
```

Using the above code, you now can call the broker factory object like so...

```csharp
var factory = container.Resolve<IBrokerObjectFactory>();
```

<br>

#### Snapshot API

Registering the Snapshot API is just as easy as registering any other HareDu API. That said, the Snapshot API is directly dependent on the Broker API so, therefore, would need said dependencies also registered. So, for the Snapshot API the registration code would look like this...
```csharp
var container = new ContainerBuilder()
                .AddHareDuConfiguration($"{Directory.GetCurrentDirectory()}/my_config.yaml")
                .AddHareDu()
                .AddHareDuSnapshot()
                .Build();
```

Using the above code, you now can call the snapshot factory object like so...

```csharp
var factory = container.Resolve<ISnapshotFactory>();
```

<br>

#### Diagnostics API

The Diagnostics API sits atop the Snapshot API, providing a means to scan snapshot data for issues. This API consists of what we call *diagnostic probes* whose purpose is to analyze a particular set of data from a particular snapshot and returns a result of that analysis back to the calling *diagnostic scan*. A diagnostic scan looks at the entire snapshot and determines which probes should be called. The ```DiagnosticScanner``` is the single point of contact to analyzing snapshot data with the Diagnostic API.


```csharp
var container = new ContainerBuilder()
                .AddHareDuConfiguration($"{Directory.GetCurrentDirectory()}/my_config.yaml")
                .AddHareDu()
                .AddHareDuSnapshot()
                .AddHareDuDiagnostics()
                .Build();
```

Using the above code, you now can call the diagnostic scanner object like so...

```csharp
var scanner = container.Resolve<IScanner>();
```
