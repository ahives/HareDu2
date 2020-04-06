# Configuration

Configuring your HareDu-powered application can be as simple as modifying the *haredu.yaml* file. At present, there are two sections in the YAML file to consider, that is, *broker* and *diagnostics*, respectively. The section called *broker* encompasses the configuration needed to use the Broker API, which all other APIs are built on top of. The section called *diagnostics* has the configuration needed to configure the Diagnostics API.

The minimum required configuration to use HareDu requires setting up communication with the RabbitMQ broker. To do so, you must have the following:

**URL (required)**

Defines the url of the RabbitMQ node that has metrics enabled.

*YAML*
```yaml
url:  http://localhost:15672
```
*C#*
```csharp
var config = provider.Configure(x =>
{
    x.Broker(y =>
    {
        y.ConnectTo("http://localhost:15672");
            ...
    });
    ...
}
```
<br>

**Username/Password (required)**

Defines the decrypted username and password of a user that has administrative access to the RabbitMQ broker.

*YAML*
```yaml
username: guest
password: guest
```
*C#*
```csharp
var config = provider.Configure(x =>
{
    x.Broker(y =>
    {
            ...
        y.UsingCredentials("guest", "guest");
    });
    ...
}
```
<br>

**Timeout (optional)**

Defines the maximum amount of time the This user should have administrative access to the RabbitMQ broker.

*YAML*
```yaml
timeout: 00:00:30
```
*C#*
```csharp
var config = provider.Configure(x =>
{
    x.Broker(y =>
    {
        y.TimeoutAfter(new TimeSpan(0, 0, 30));
            ...
    });
    ...
}
```

<br>

#### Configuring Diagnostic Probes

The Diagnostics API is configured under the following section...

*YAML*

```yaml
  diagnostics:
    probes:
```
<br>

**HighConnectionClosureRateProbe**

Defines the maximum acceptable rate of which connections are closed on the RabbitMQ broker. If the rate of which connections are closed is greater than or equal to this setting, a warning is generated, which implies that the application communicating with the broker may be experiencing issues. If the rate of closed connections is less than this setting then the system is considered to be operating normally.

*YAML*
```yaml
high-connection-closure-rate-threshold:  100
```

*C#*
```csharp
var config = provider.Configure(x =>
{
    ...
    
    x.Diagnostics(y =>
    {
        y.Probes(z =>
        {
            z.SetHighConnectionClosureRateThreshold(100);
                ...
        });
    });
});
```
<br>

**HighConnectionCreationRateProbe**

Defines the maximum acceptable rate of which connections to the RabbitMQ broker can be established in order to determine whether or not it is considered healthy. If the rate of which connections are created is greater than or equal to this setting, a warning is generated, which implies that the application communicating with the broker may be experiencing issues. Otherwise, if the rate of created connections is less than this setting then the system is consider to be operating normally.

*YAML*
```yaml
high-connection-creation-rate-threshold: 100
```

*C#*
```csharp
var config = provider.Configure(x =>
{
        ...
    
    x.Diagnostics(y =>
    {
        y.Probes(z =>
        {
            z.SetHighConnectionCreationRateThreshold(100);
                ...
        });
    });
});
```
<br>

**QueueHighFlowProbe**

Defines the maximum acceptable number of messages that can be published to a queue. If the number of published messages is greater than or equal to this setting, then, a queue is considered unhealthy. Otherwise, the queue is considered healthy.

*YAML*
```yaml
queue-high-flow-threshold:  100
```

*C#*
```csharp
var config = provider.Configure(x =>
{
        ...
    
    x.Diagnostics(y =>
    {
        y.Probes(z =>
        {
            z.SetQueueHighFlowThreshold(100);
                ...
        });
    });
});

```
<br>

**QueueLowFlowProbe**

Defines the minimum acceptable number of messages that is published to a queue. If the number of published messages is less than or equal to this setting, then, a queue is considered unhealthy. Otherwise, the queue is considered healthy.

*YAML*
```yaml
queue-low-flow-threshold: 20
```

*C#*
```csharp
var config = provider.Configure(x =>
{
        ...
    
    x.Diagnostics(y =>
    {
        y.Probes(z =>
        {
            z.SetQueueLowFlowThreshold(20);
                ...
        });
    });
});
```
<br>

**RedeliveredMessagesProbe**

Defines the coefficient that will be used to calculate the acceptable number of message redelivers. A fractional value of 1 or greater will result in the calculated threshold being equal to the number of incoming messages for a particular queue. A fractional value less than 1 will result in the calculated threshold being derived from said value times the amount of incoming messages. The resultant value will determine whether the corresponding RabbitMQ component is *healthy*, *unhealthy*, or *warning*.

*YAML*
```yaml
message-redelivery-threshold-coefficient: 0.50
```

*C#*
```csharp
var config = provider.Configure(x =>
{
        ...
    
    x.Diagnostics(y =>
    {
        y.Probes(z =>
        {
            z.SetMessageRedeliveryThresholdCoefficient(0.50M);
                ...
        });
    });
});
```
<br>

**SocketDescriptorThrottlingProbe**

Defines the coefficient that will be used to calculate the acceptable number of sockets that can be used. A fractional value of 1 or greater will result in the calculated threshold being equal to the number of available sockets. A fractional value less than 1 will result in the calculated threshold being derived from said value times the number of available sockets. The resultant value will determine whether the corresponding RabbitMQ component is *healthy*, *unhealthy*, or *warning*.

*YAML*
```yaml
socket-usage-threshold-coefficient: 0.60
```

*C#*
```csharp
var config = provider.Configure(x =>
{
        ...
    
    x.Diagnostics(y =>
    {
        y.Probes(z =>
        {
            z.SetSocketUsageThresholdCoefficient(0.60M);
                ...
        });
    });
});
```

<br>

**RuntimeProcessLimitProbe**

Defines the coefficient that will be used to calculate the acceptable number of runtime processes that can be used. A fractional value of 1 or greater will result in the calculated threshold being equal to the upper limit of available runtime processes. A fractional value less than 1 will result in the calculated threshold being derived from said value times the predefined upper limit of available runtime processes. The resultant value will determine whether the corresponding RabbitMQ component is *healthy*, *unhealthy*, or *warning*.

*YAML*
```yaml
runtime-process-usage-threshold-coefficient:  0.65
```

*C#*
```csharp
var config = provider.Configure(x =>
{
        ...
    
    x.Diagnostics(y =>
    {
        y.Probes(z =>
        {
            z.SetRuntimeProcessUsageThresholdCoefficient(0.65M);
                ...
        });
    });
});
```
<br>

**FileDescriptorThrottlingProbe**

Defines the coefficient that will be used to calculate the acceptable number of file descriptors/handles that can be used. A fractional value of 1 or greater will result in the calculated threshold being equal to the number of available file descriptors/handles. A fractional value less than 1 will result in the calculated threshold being derived from said value times the number of available file descriptors/handles. The resultant value will determine whether the corresponding RabbitMQ component is *healthy*, *unhealthy*, or *warning*.

*YAML*
```yaml
file-descriptor-usage-threshold-coefficient:  0.65
```

*C#*
```csharp
var config = provider.Configure(x =>
{
        ...
    
    x.Diagnostics(y =>
    {
        y.Probes(z =>
        {
            z.SetFileDescriptorUsageThresholdCoefficient(0.65M);
                ...
        });
    });
});
```
<br>

**ConsumerUtilizationProbe**

Defines the minimum acceptable percentage of consumers that are consuming messages from a particular queue. This value along with the consumer utilization data will determine whether the corresponding RabbitMQ component is *healthy*, *unhealthy*, or *warning*.

*YAML*
```yaml
consumer-utilization-threshold: 0.50
```

*C#*
```csharp
var config = provider.Configure(x =>
{
        ...
    
    x.Diagnostics(y =>
    {
        y.Probes(z =>
        {
            z.SetConsumerUtilizationThreshold(0.50M);
                ...
        });
    });
});
```
<br>

#### Putting it Altogether

The combined YAML configuration looks like this...
```yaml
---
  broker:
      url:  http://localhost:15672
      username: guest
      password: guest
      timeout: 00:00:30
  diagnostics:
    probes:
        high-connection-closure-rate-threshold:  100
        high-connection-creation-rate-threshold: 100
        queue-high-flow-threshold:  100
        queue-low-flow-threshold: 20
        message-redelivery-threshold-coefficient: 0.50
        socket-usage-threshold-coefficient: 0.60
        runtime-process-usage-threshold-coefficient:  0.65
        file-descriptor-usage-threshold-coefficient:  0.65
        consumer-utilization-threshold: 0.50
...
```

To access the above YAML configuration you can either read said configuration from a file or text.  

If reading from a file, you need to initialize ```YamlFileConfigProvider```...

*Do it yourself*
```csharp
var provider = new YamlFileConfigProvider();
```
<br>

*Autofac*
```csharp
var provider = _container.Resolve<IFileConfigProvider>();
```
<br>

*.NET Core DI*
```csharp
var provider = _services.GetService<IFileConfigProvider>();
```
<br>

If reading text you need to initialize ```YamlConfigProvider```...

*Do it yourself*
```csharp
var provider = new YamlConfigProvider();
```
<br>

*Autofac*
```csharp
var provider = _container.Resolve<IConfigProvider>();
```
<br>

*.NET Core DI*
```csharp
var provider = _services.GetService<IConfigProvider>();
```
<br>

Depending on whether you are using the configuration or file provider, you would call the appropriate ```TryGet``` method to return the configuration.

<br>

There are several ways to configure HareDu programmatically. Let's look at the major scenarios.

<br>

**Dude, I don't care about YAML**

First, you need to initialize ```HareDuConfigProvider```...

*Do it yourself*
```csharp
var provider = new HareDuConfigProvider();
```

*Autofac*
```csharp
var provider = _container.Resolve<IHareDuConfigProvider>();
```

*.NET Core DI*
```csharp
var provider = _services.GetService<IHareDuConfigProvider>();
```
<br>

From here you can use the initialized provider to set configuration settings on all the HareDu APIs like so...
```csharp
var config = provider.Configure(x =>
{
    x.Broker(y =>
    {
        y.ConnectTo("http://localhost:15672");
        y.UsingCredentials("guest", "guest");
        y.TimeoutAfter(new TimeSpan(0, 0, 30));
    });

    x.Diagnostics(y =>
    {
        y.Probes(z =>
        {
            z.SetMessageRedeliveryThresholdCoefficient(0.60M);
            z.SetSocketUsageThresholdCoefficient(0.60M);
            z.SetConsumerUtilizationThreshold(0.65M);
            z.SetQueueHighFlowThreshold(90);
            z.SetQueueLowFlowThreshold(10);
            z.SetRuntimeProcessUsageThresholdCoefficient(0.65M);
            z.SetFileDescriptorUsageThresholdCoefficient(0.65M);
            z.SetHighConnectionClosureRateThreshold(90);
            z.SetHighConnectionCreationRateThreshold(60);
        });
    });
});
```
<br>

From here, you need only call ```config.Broker``` or ```config.Diagnostics``` to access configuration data.

It is assumed that you will not need to change settings after API objects are initialized. HareDu is an immutable API so it is recommended that most API objects be initialized using the Singleton pattern. Because of this, it can be difficult to update configuration. Don't worry, HareDu got you covered! HareDu, not only allows you to safely update configuration after initialization, it also allows you to register observers that will notify you of changes to the original configuration settings.

Updating configuration is as easy as calling the appropriate ```UpdateConfiguration``` method on the corresponding object.

Next, create a class that implements ```IObserver<T>``` like so...
```csharp
public class ConfigOverrideObserver :
    IObserver<ProbeConfigurationContext>
{
    public void OnCompleted() => throw new NotImplementedException();

    public void OnError(Exception error) => throw new NotImplementedException();

    public void OnNext(ProbeConfigurationContext value) => throw new NotImplementedException();
}
```
Call the ```RegisterObserver``` method on ```ScannerFactory``` like so...
```csharp
factory.RegisterObserver(new ConfigOverrideObserver());
```
Done.
<br>

*Please note that the above functionality currently only works with the Diagnostics API.*
