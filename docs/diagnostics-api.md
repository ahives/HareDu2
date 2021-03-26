# Diagnostics API

The Diagnostics API sits atop the Snapshot API, providing a means to scan snapshot data for issues. This API consists of what we call *diagnostic probes* whose purpose is to analyze a particular set of data from a particular snapshot and returns a result of that analysis back to the calling *diagnostic scan*. A diagnostic scan looks at the entire snapshot and determines which probes should be called. The ```DiagnosticScanner``` is the single point of contact to analyzing snapshot data with the Diagnostic API.

#### Registering API objects
The very first thing you need to do is register/initialize the appropriate objects you will need to perform diagnostic scans on snapshot data captured from the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported DI containers. Currently, HareDu 3 supports only two DI containers; Autofac and Microsoft, respectively.

<br>

Registering objects without DI containers is pretty simple as well...

```c#
var scanner = new Scanner(factory);
```
Since the ```Scanner``` should only be initialized once in your application, therefore, you should use the Singleton pattern. Please note that the IoC integrations registers ```Scanner``` as a singleton. This applies to most things in HareDu 2.

#### Scanning snapshots

```c#
var result = scanner.Scan(snapshot);
```

The above code will return a ```ScannerResult``` object, which contains the result of executing each diagnostic probe against the snapshot data.

#### Registering Observers

When setting up ```Scanner```, you can register observers. These observers should implement ```IObserver<T>``` where ```T``` is ```SnapshotContext<T>```. Each time a snapshot is taken (i.e. when the ```Scan``` method is called), all registered observers will be notified with an object of ```SnapshotContext<T>```. Registering an observer is easy enough (see code snippet below) but be sure to do so before calling the ```Scan``` method or else the registered observers will not receive notifications.

```c#
var result = scanner
    .RegisterObserver(new SomeCoolObserver())
    .Scan(snapshot);
```
<br>

#### Putting it altogether

Below is an example of scanning a ```BrokerQueues``` snapshot.

```c#
// Define the scanner observer
public class BrokerQueuesScannerObserver :
    IObserver<ProbeContext>
{
    public void OnCompleted() => throw new NotImplementedException();

    public void OnError(Exception error) => throw new NotImplementedException();

    public void OnNext(ProbeContext value) => throw new NotImplementedException();
}

// Get the configuration
HareDuConfig config = new HareDuConfig();

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .Build();

configuration.Bind("HareDuConfig", config);

// Initialize a snapshot factory
var snapshotFactory = new SnapshotFactory(new BrokerObjectFactory(config))
    .Lens<BrokerQueuesSnapshot>();
    
// Get a snapshot
var snapshot = await snapshotFactory.TakeSnapshot();

// Initialize a diagnostic scanner
var scanner = new Scanner(new ScannerFactory(config, new KnowledgeBaseProvider()))
    .RegisterObserver(new BrokerQueuesScannerObserver());

// Execute the scanner against the snapshot
var result = scanner.Scan(snapshot);
```
