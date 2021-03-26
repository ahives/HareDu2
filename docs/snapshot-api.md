# Snapshot API

The Snapshot API sits atop the Broker API and provides a high level rollup of RabbitMQ broker metrics. Each snapshot makes one or more calls to the Broker API methods aggregating the metric data into a developer-friendly object. Each snapshot is then captured on a timeline that can be then flushed to disk or saved to a database.

#### Registering API objects
The very first thing you need to do is register/initialize the appropriate objects you will need to take snapshots metric data on the RabbitMQ broker. To do that you have two options, that is, initialize the objects yourself, managing the associated lifetime scopes of said objects or use one of the supported DI containers. Currently, HareDu 2 supports only two DI containers; Autofac and Microsoft, respectively.

#### Taking snapshots
Once you have registered a ```SnapshotFactory```, it is easy to take a snapshot.

**Step 1: Define what type of snapshot you want to take**
```c#
var lens = new SnapshotFactory(config)
    .Lens<BrokerQueuesSnapshot>();
```
In this code snippet, the lens variable returns a ``SnapshotLens`` for taking snapshots of type ```BrokerQueuesSnapshot```.

**Step 2: Take the snapshot**
```c#
var result = lens.TakeSnapshot();
```

#### Viewing snapshot history

Snapshots are accessible via ```History```. Getting the most recent snapshot is as easy as calling the ```MostRecent``` extension method off of the ```History``` property on the lens like so...  

```c#
var result = lens.History.MostRecent();
```
Since ```MostRecent``` returns a ```SnapshotResult<T>``` you can easily get the actual snapshot data by simple calling the ```Snapshot``` property like this...

```c#
var snapshot = result.Snapshot;
```
...or more concisely...

```c#
var snapshot = lens.History.MostRecent().Snapshot;
```

#### Registering Observers

When setting up ```SnapshotFactory```, you can register observers. These observers should implement ```IObserver<T>``` where ```T``` is ```SnapshotResult<T>```. Each time a snapshot is taken (i.e. when the ```TakeSnapshot``` method is called), all registered observers will be notified with an object of ```SnapshotResult<T>```. Registering an observer is easy enough (see code snippet below) but be sure to do so before calling the ```TakeSnapshot``` method.

```c#
var lens = new SnapshotFactory(config)
    .Lens<BrokerQueuesSnapshot>()
    .RegisterObserver(new SomeCoolObserver())
    .RegisterObserver(new SomeOtherCoolObserver());
```

#### Putting it altogether
Below is an example of taking a snapshot of RabbitMQ broker queue data, registering an observer to receive notifications when said snapshot is taken, and returning the most recent snapshot from the snapshot timeline.

```c#
// Define the snapshot observer
public class BrokerQueuesObserver :
    IObserver<SnapshotContext<BrokerQueuesSnapshot>>
{
    public void OnCompleted() => throw new NotImplementedException();

    public void OnError(Exception error) => throw new NotImplementedException();

    public void OnNext(SnapshotContext<BrokerQueuesSnapshot> value) => throw new NotImplementedException();
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
```
