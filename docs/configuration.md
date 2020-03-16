# Configuration
Configuring your HareDu-powered application can be as simple as modifying the *haredu.yaml* file. At present, there are two sections in the YAML file to consider, that is, *broker* and *diagnostics*, respectively. The section called *broker* encompasses the configuration needed to use the Broker API. The section called *diagnostics* has the configuration needed to use the Diagnostics API.

| Setting | Description |
| ---| --- |
| **Broker** | |
| url | The url of the RabbitMQ node that has metrics enabled. |
| username | This user should have administrative access to the RabbitMQ broker. |
| password | Decrypted password of a user that has administrative access to the RabbitMQ broker. |
| **Diagnostics** | |
| high-closure-rate-warning-threshold | Defines the maximum acceptable rate of which connections are closed on the RabbitMQ broker to determine whether or not it is considered healthy. If the rate of which connections are closed is greater than or equal to this setting, a warning is generated, which implies that the application communicating with the broker may be experiencing issues. Otherwise, if the rate of closed connections is less than this setting then the system is considered to be operating normally. |
| high-creation-rate-warning-threshold | Defines the maximum acceptable rate of which connections to the RabbitMQ broker can be made in order to determine whether or not it is considered healthy. If the rate of which connections are created is greater than or equal to this setting, a warning is generated, which implies that the application communicating with the broker may be experiencing issues. Otherwise, if the rate of created connections is less than this setting then the system is consider to be operating normally. |
| queue-high-flow-threshold | Defines the maximum acceptable number of messages that can be published to a queue. If the number of published messages is greater than or equal to this setting then this queue is considered unhealthy  |
| queue-low-flow-threshold |  |
| message-redelivery-threshold-coefficient | Is used to help calculate |
| socket-usage-coefficient |  |
| runtime-process-usage-coefficient |  |
| file-descriptor-usage-warning-coefficient |  |
| consumer-utilization-warning-coefficient |  |

The minimum of using HareDu requires setting up communication with the RabbitMQ broker. To do so, you must
```yaml
  broker:
      url:  http://localhost:15672
      username: guest
      password: guest
      timeout: 00:00:30
```

The Diagnostics API is configured under the following section
```yaml
  diagnostics:
    probes:
```

##### Configuring HighConnectionClosureRateProbe
Defines the maximum acceptable rate of which connections are closed on the RabbitMQ broker to determine whether or not it is considered healthy. If the rate of which connections are closed is greater than or equal to this setting, a warning is generated, which implies that the application communicating with the broker may be experiencing issues. Otherwise, if the rate of closed connections is less than this setting then the system is considered to be operating normally.

*YAML*
```yaml
high-closure-rate-threshold:  100
```

*C#*
```csharp
var config = provider.Configure(x => x.SetHighClosureRateThreshold(100));
```

##### Configuring HighConnectionCreationRateProbe
Defines the maximum acceptable rate of which connections to the RabbitMQ broker can be made in order to determine whether or not it is considered healthy. If the rate of which connections are created is greater than or equal to this setting, a warning is generated, which implies that the application communicating with the broker may be experiencing issues. Otherwise, if the rate of created connections is less than this setting then the system is consider to be operating normally.

*YAML*
```yaml
high-creation-rate-threshold: 100
```

*C#*
```csharp
var config = provider.Configure(x => x.SetHighCreationRateThreshold(100));
```


##### Configuring QueueHighFlowProbe
Defines the maximum acceptable number of messages that can be published to a queue. If the number of published messages is greater than or equal to this setting then this queue is considered unhealthy

*YAML*
```yaml
queue-high-flow-threshold:  100
```

*C#*
```csharp
var config = provider.Configure(x => x.SetQueueHighFlowThreshold(100));
```

##### Configuring QueueLowFlowProbe

*YAML*
```yaml
queue-low-flow-threshold: 20
```

*C#*
```csharp
var config = provider.Configure(x => x.SetQueueLowFlowThreshold(20));
```

##### Configuring RedeliveredMessagesProbe

*YAML*
```yaml
message-redelivery-threshold-coefficient: 0.50
```

*C#*
```csharp
var config = provider.Configure(x => x.SetMessageRedeliveryThresholdCoefficient(0.50M));
```

##### Configuring SocketDescriptorThrottlingProbe

*YAML*
```yaml
socket-usage-threshold-coefficient: 0.60
```

*C#*
```csharp
var config = provider.Configure(x => x.SetSocketUsageThresholdCoefficient(0.60M));
```

##### Configuring RuntimeProcessLimitProbe

*YAML*
```yaml
runtime-process-usage-threshold-coefficient:  0.65
```

*C#*
```csharp
var config = provider.Configure(x => x.SetRuntimeProcessUsageThresholdCoefficient(0.65M));
```

##### Configuring FileDescriptorThrottlingProbe

*YAML*
```yaml
file-descriptor-usage-threshold-coefficient:  0.65
```

*C#*
```csharp
var config = provider.Configure(x => x.SetFileDescriptorUsageThresholdCoefficient(0.65M));
```

##### Configuring ConsumerUtilizationProbe

*YAML*
```yaml
consumer-utilization-threshold: 0.50
```

*C#*
```csharp
var config = provider.Configure(x => x.SetConsumerUtilizationThreshold(0.50M));
```

HareDu YAML looks like this...
```yaml
---
  broker:
      url:  http://localhost:15672
      username: guest
      password: guest
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

There are several ways to configure HareDu. Let's look at the major scenarios.

#### I just want to configure the Broker/Snapshot API
You can either do this explicitly by calling the ```BrokerConfigProvider``` directly like so...
```csharp
var provider = new BrokerConfigProvider();
var config = provider.Configure(x =>
{
    x.ConnectTo("http://localhost:15672");
    x.UsingCredentials("guest", "guest");
});
```
...or you can simply read YAML configuration. There are two ways to read YAML. Either you can read a YAML configuration file like so...
```csharp
var validator = new YourCustomConfigValidator();
var provider = new YamlFileConfigProvider(validator);

provider.TryGet("haredu.yaml", out HareDuConfig config);
```
...or you can read YAML text like this...
```csharp
var validator = new YourCustomConfigValidator();
var provider = new YamlConfigProvider(validator);

provider.TryGet(yamlText, out HareDuConfig config);
```
From here you need only call ```config.Broker``` to access the broker configuration. In the above example, ```YamlFileConfigProvider``` and ```YamlConfigProvider``` are shown to be initialized by explicitly passing ```IConfigValidator```, which can be a custom validator. However, if you want to use the default validator then use the parameterless constructor.
<br>

#### I just want to configure the Diagnostics API
There are a couple ways to configure the Diagnostics API. Since most of the default diagnostic probes are configurable by passing in settings, we give you a way to codify those settings. The first option is to read the *haredu.yaml* file. The other option is set the configuration explicitly like so...

```csharp
var provider = new DiagnosticsConfigProvider();
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
