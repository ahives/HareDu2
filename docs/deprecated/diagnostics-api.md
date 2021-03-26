# Diagnostics API

The Diagnostics API sits atop the Snapshot API, providing a means to scan snapshot data for issues. This API consists of what we call *diagnostic probes* whose purpose is to analyze a particular set of data from a particular snapshot and returns a result of that analysis back to the calling *diagnostic scan*. A diagnostic scan looks at the entire snapshot and determines which probes should be called. The ```DiagnosticScanner``` is the single point of contact to analyzing snapshot data with the Diagnostic API.

#### Registering API objects
The very first thing you need to do is register/initialize the appropriate objects you will need to perform diagnostic scans on snapshot data captured from the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported IoC containers. Currently, HareDu 2 supports only two IoC containers; Autofac and .NET Core, respectively.

Note: The IoC container code that comes with HareDu currently defaults to file based configuration so you will need to make the appropriate changes to the haredu.yaml file.

<br>

Registering objects without IoC containers is pretty simple as well...

```csharp
var scanner = new Scanner(factory);
```
Since the ```Scanner``` should only be initialized once in your application, therefore, you should use the Singleton pattern. Please note that the IoC integrations registers ```Scanner``` as a singleton. This applies to most things in HareDu 2.

#### Scanning snapshots

```csharp
var result = scanner.Scan(snapshot);
```

The above code will return a ```ScannerResult``` object, which contains the result of executing each diagnostic probe against the snapshot data.

#### Registering Observers

When setting up ```Scanner```, you can register observers. These observers should implement ```IObserver<T>``` where ```T``` is ```SnapshotContext<T>```. Each time a snapshot is taken (i.e. when the ```Scan``` method is called), all registered observers will be notified with an object of ```SnapshotContext<T>```. Registering an observer is easy enough (see code snippet below) but be sure to do so before calling the ```Scan``` method or else the registered observers will not receive notifications.

```csharp
var result = scanner
    .RegisterObserver(new SomeCoolObserver())
    .Scan(snapshot);
```
<br>

#### Putting it altogether

Below is an example of scanning a ```BrokerQueues``` snapshot.

```csharp
// Define the scanner observer
public class BrokerQueuesScannerObserver :
    IObserver<ProbeContext>
{
    public void OnCompleted() => throw new NotImplementedException();

    public void OnError(Exception error) => throw new NotImplementedException();

    public void OnNext(ProbeContext value) => throw new NotImplementedException();
}

var provider = new YamlFileConfigProvider();

// Get the API configuration
provider.TryGet("haredu.yaml", out HareDuConfig config);

var snapshotFactory = new SnapshotFactory(config);

// Take a snapshot
var lens = snapshotFactory.Lens<BrokerQueuesSnapshot>();
var snapshotResult = lens.TakeSnapshot();

var scannerFactory = new ScannerFactory(config, new KnowledgeBaseProvider());

// Initialize the diagnostic scanner and register the observer
var scanner = new Scanner(scannerFactory)
    .RegisterObserver(new BrokerQueuesScannerObserver());

// Scan the results of the most recent snapshot taken
var result = scanner.Scan(snapshotResult.Snapshot);
```
