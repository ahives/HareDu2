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
var config = provider.Configure(x => x.ConnectTo("http://localhost:15672"));
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
var config = provider.Configure(x => x.UsingCredentials("guest", "guest"));
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
var config = provider.Configure(x => x.TimeoutAfter(new TimeSpan(0, 0, 30)));
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
high-closure-rate-threshold:  100
```

*C#*
```csharp
var config = provider.Configure(x => x.SetHighClosureRateThreshold(100));
```
<br>

**HighConnectionCreationRateProbe**

Defines the maximum acceptable rate of which connections to the RabbitMQ broker can be established in order to determine whether or not it is considered healthy. If the rate of which connections are created is greater than or equal to this setting, a warning is generated, which implies that the application communicating with the broker may be experiencing issues. Otherwise, if the rate of created connections is less than this setting then the system is consider to be operating normally.

*YAML*
```yaml
high-creation-rate-threshold: 100
```

*C#*
```csharp
var config = provider.Configure(x => x.SetHighCreationRateThreshold(100));
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
var config = provider.Configure(x => x.SetQueueHighFlowThreshold(100));
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
var config = provider.Configure(x => x.SetQueueLowFlowThreshold(20));
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
var config = provider.Configure(x => x.SetMessageRedeliveryThresholdCoefficient(0.50M));
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
var config = provider.Configure(x => x.SetSocketUsageThresholdCoefficient(0.60M));
```

<br>

**RuntimeProcessLimitProbe**

*YAML*
```yaml
runtime-process-usage-threshold-coefficient:  0.65
```

*C#*
```csharp
var config = provider.Configure(x => x.SetRuntimeProcessUsageThresholdCoefficient(0.65M));
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
var config = provider.Configure(x => x.SetFileDescriptorUsageThresholdCoefficient(0.65M));
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
var config = provider.Configure(x => x.SetConsumerUtilizationThreshold(0.50M));
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
        high-closure-rate-threshold:  100
        high-creation-rate-threshold: 100
        queue-high-flow-threshold:  100
        queue-low-flow-threshold: 20
        message-redelivery-threshold-coefficient: 0.50
        socket-usage-threshold-coefficient: 0.60
        runtime-process-usage-threshold-coefficient:  0.65
        file-descriptor-usage-threshold-coefficient:  0.65
        consumer-utilization-threshold: 0.50
...
```

There are several ways to configure HareDu programmatically. Let's look at the major scenarios.

<br>

**Dude, I just want to configure the Broker/Snapshot API**

You can either do this explicitly by calling the ```BrokerConfigProvider``` directly like so...

*Do it yourself*
```csharp
var provider = new BrokerConfigProvider();
```
<br>

*Autofac*

```csharp
var provider = _container.Resolve<IBrokerConfigProvider>();
```
<br>

*.NET Core DI*
```csharp
var provider = _services.GetService<IBrokerConfigProvider>();
```
<br>

From here you can use the provider to set configuration settings on the Broker API like this...
```csharp
var config = provider.Configure(x =>
{
    x.ConnectTo("http://localhost:15672");
    x.UsingCredentials("guest", "guest");
    x.TimeoutAfter(new TimeSpan(0, 0, 30));
});
```

...or you can simply read YAML configuration. There are two ways to read YAML. Either you can read a YAML configuration file like so...
```csharp
var validator = new YourCustomConfigValidator();
var provider = new YamlFileConfigProvider(validator);

provider.TryGet("haredu.yaml", out HareDuConfig config);
```
<br>

...or you can read YAML text like this...
```csharp
var validator = new YourCustomConfigValidator();
var provider = new YamlConfigProvider(validator);

provider.TryGet(yaml, out HareDuConfig config);
```
<br>

...or you can always use supported IoC containers like this...

*Autofac*
```csharp
var provider = _container.Resolve<IFileConfigProvider>();
```
...or
```csharp
var provider = _container.Resolve<IConfigProvider>();
```
<br>

*.NET Core DI*
```csharp
var provider = _services.GetService<IFileConfigProvider>();
```
...or
```csharp
var provider = _services.GetService<IConfigProvider>();
```

Depending on whether you are using the configuration or file provider, you would call the appropriate ```TryGet``` method to return the configuration. From here, you need only call ```config.Broker``` to access the broker configuration.  

<br>

**Dude, I just want to configure the Diagnostics API**

There are a couple ways to configure the Diagnostics API. Since most of the default diagnostic probes are configurable by passing in settings, we give you a way to codify those settings. The first option is to read the *haredu.yaml* file. The other option is set the configuration explicitly like so...

*Do it yourself*
```csharp
var provider = new DiagnosticsConfigProvider();
```
<br>

*Autofac*

```csharp
var provider = _container.Resolve<IDiagnosticsConfigProvider>();
```
<br>

*.NET Core DI*
```csharp
var provider = _services.GetService<IDiagnosticsConfigProvider>();
```
<br>

From here you can use the provider to set configuration settings on the Diagnostics API like this...
```csharp
var config = provider.Configure(x =>
            {
                x.SetMessageRedeliveryThresholdCoefficient(0.60M);
                x.SetSocketUsageThresholdCoefficient(0.60M);
                x.SetConsumerUtilizationThreshold(0.65M);
                x.SetQueueHighFlowThreshold(90);
                x.SetQueueLowFlowThreshold(10);
                x.SetRuntimeProcessUsageThresholdCoefficient(0.65M);
                x.SetFileDescriptorUsageThresholdCoefficient(0.65M);
                x.SetHighClosureRateThreshold(90);
                x.SetHighCreationRateThreshold(60);
            });

```
<br>

...or you can always use supported IoC containers like this...

*Autofac*
```csharp
var provider = _container.Resolve<IFileConfigProvider>();
```
...or
```csharp
var provider = _container.Resolve<IConfigProvider>();
```
<br>

*.NET Core DI*
```csharp
var provider = _services.GetService<IFileConfigProvider>();
```
...or
```csharp
var provider = _services.GetService<IConfigProvider>();
```
