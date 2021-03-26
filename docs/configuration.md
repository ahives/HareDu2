# Configuration

Configuring your HareDu-powered application can be as simple as modifying your appsettings.json file by adding a HareDu configuration section. At present, there are two major sections within the settings file to consider, that is, *broker* and *diagnostics*, respectively. The section called *broker* encompasses the configuration needed to use the Broker API, which all other APIs are built on top of. The section called *diagnostics* has the configuration needed to configure the Diagnostics API.

The minimum required configuration to use HareDu requires setting up communication with the RabbitMQ broker. To do so, you must have the following:

**URL (required)**

Defines the url of the RabbitMQ node that has metrics enabled.

*JSON*
```json
"Url": "http://localhost:15672"
```
*C#*
```c#
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

*JSON*
```json
"Credentials": {
    "Username": "guest",
    "Password": "guest"
}
 ```

*C#*
```c#
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

*JSON*
```json
"Timeout": 00:00:30
```
*C#*
```c#
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

*JSON*

```json
"Diagnostics": {
  "Probes": {
    ...
    
  }
}
```
<br>

**HighConnectionClosureRateProbe**

Defines the maximum acceptable rate at which connections can be closed on the RabbitMQ broker. If the rate of which connections are closed is greater than or equal to this setting, a warning is generated, which implies that the application communicating with the broker may be experiencing issues. If the rate of closed connections is less than this setting then the system is considered to be operating normally.

*JSON*
```json
"HighConnectionClosureRateThreshold": 100
```

*C#*
```c#
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

Defines the maximum acceptable rate at which connections to the RabbitMQ broker can be established in order to determine whether or not it is considered healthy. If the rate of which connections are created is greater than or equal to this setting, a warning is generated, which implies that the application communicating with the broker may be experiencing issues. Otherwise, if the rate of created connections is less than this setting then the system is consider to be operating normally.

*JSON*
```json
"HighConnectionCreationRateThreshold": 100
```

*C#*
```c#
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

*JSON*
```json
"QueueHighFlowThreshold": 100
```

*C#*
```c#
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

Defines the minimum acceptable number of messages that can be published to a queue. If the number of published messages is less than or equal to this setting, then, a queue is considered unhealthy. Otherwise, the queue is considered healthy.

*JSON*
```json
"QueueLowFlowThreshold": 20
```

*C#*
```c#
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

*JSON*
```json
"MessageRedeliveryThresholdCoefficient": 0.50
```

*C#*
```c#
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

Defines the coefficient used to calculate the acceptable number of sockets that can be used. A fractional value of 1 or greater will result in the calculated threshold being equal to the number of available sockets. A fractional value less than 1 will result in the calculated threshold being derived from said value times the number of available sockets. The resultant value will determine whether the corresponding RabbitMQ component is *healthy*, *unhealthy*, or *warning*.

*JSON*
```json
"SocketUsageThresholdCoefficient": 0.60
```

*C#*
```c#
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

Defines the coefficient used to calculate the acceptable number of runtime processes that can be used. A fractional value of 1 or greater will result in the calculated threshold being equal to the upper limit of available runtime processes. A fractional value less than 1 will result in the calculated threshold being derived from said value times the predefined upper limit of available runtime processes. The resultant value will determine whether the corresponding RabbitMQ component is *healthy*, *unhealthy*, or *warning*.

*JSON*
```json
"RuntimeProcessUsageThresholdCoefficient": 0.65
```

*C#*
```c#
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

Defines the coefficient used to calculate the acceptable number of file descriptors/handles that can be used. A fractional value of 1 or greater will result in the calculated threshold being equal to the number of available file descriptors/handles. A fractional value less than 1 will result in the calculated threshold being derived from said value times the number of available file descriptors/handles. The resultant value will determine whether the corresponding RabbitMQ component is *healthy*, *unhealthy*, or *warning*.

*JSON*
```json
"FileDescriptorUsageThresholdCoefficient": 0.65
```

*C#*
```c#
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

*JSON*
```json
"ConsumerUtilizationThreshold": 0.50
```

*C#*
```c#
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

The combined JSON configuration looks like this...
```json
  "HareDuConfig": {
    "Broker": {
      "Url": "http://localhost:15672",
      "Credentials": {
        "Username": "guest",
        "Password": "guest"
      }
    },
    "Diagnostics": {
      "Probes": {
        "HighConnectionClosureRateThreshold": 100,
        "HighConnectionCreationRateThreshold": 100,
        "QueueHighFlowThreshold": 100,
        "QueueLowFlowThreshold": 20,
        "MessageRedeliveryThresholdCoefficient": 0.50,
        "SocketUsageThresholdCoefficient": 0.60,
        "RuntimeProcessUsageThresholdCoefficient": 0.65,
        "FileDescriptorUsageThresholdCoefficient": 0.65,
        "ConsumerUtilizationThreshold": 0.50
      }
    }
  }
```

If reading from a file, you need to add the following code...

*Do it yourself*
```c#
HareDuConfig config = new HareDuConfig();
            
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .Build();

configuration.Bind("HareDuConfig", config);
```

*Using Dependency Injection*

```c#
.AddHareDu()
```
...or  
```c#
.AddHareDu("your_settings_file.json", "settings_file_config_section")
```
<br>

**Dude, I don't care about configuration files**

Well, first you need to initialize ```HareDuConfigProvider```...

*Do it yourself*
```c#
var provider = new HareDuConfigProvider();
```

From here you can use the initialized provider to set configuration settings on all the HareDu APIs like so...
```c#
var config = provider.Configure(x =>
{
    x.Broker(broker =>
    {
        broker.ConnectTo("http://localhost:15672");
        broker.UsingCredentials("guest", "guest");
        broker.TimeoutAfter(new TimeSpan(0, 0, 30));
    });

    x.Diagnostics(diagnostic =>
    {
        diagnostic.Probes(probe =>
        {
            probe.SetMessageRedeliveryThresholdCoefficient(0.60M);
            probe.SetSocketUsageThresholdCoefficient(0.60M);
            probe.SetConsumerUtilizationThreshold(0.65M);
            probe.SetQueueHighFlowThreshold(90);
            probe.SetQueueLowFlowThreshold(10);
            probe.SetRuntimeProcessUsageThresholdCoefficient(0.65M);
            probe.SetFileDescriptorUsageThresholdCoefficient(0.65M);
            probe.SetHighConnectionClosureRateThreshold(90);
            probe.SetHighConnectionCreationRateThreshold(60);
        });
    });
});
```

...and now you need only call ```config.Broker``` or ```config.Diagnostics``` to access configuration data.

<br>

*Using Dependency Injection*

```c#
.AddHareDu(x =>
    {
        x.Broker(broker =>
        {
            broker.ConnectTo("http://localhost:15672");
            broker.UsingCredentials("guest", "guest");
            broker.TimeoutAfter(new TimeSpan(0, 0, 30));
        });

        x.Diagnostics(diagnostic =>
        {
            diagnostic.Probes(probe =>
            {
                probe.SetMessageRedeliveryThresholdCoefficient(0.60M);
                probe.SetSocketUsageThresholdCoefficient(0.60M);
                probe.SetConsumerUtilizationThreshold(0.65M);
                probe.SetQueueHighFlowThreshold(90);
                probe.SetQueueLowFlowThreshold(10);
                probe.SetRuntimeProcessUsageThresholdCoefficient(0.65M);
                probe.SetFileDescriptorUsageThresholdCoefficient(0.65M);
                probe.SetHighConnectionClosureRateThreshold(90);
                probe.SetHighConnectionCreationRateThreshold(60);
            });
        });
    })
```

...and now you need only reference ```HareDuConfig``` by either referencing within your constructor or resolving the object from the DI container like so...

**Autofac**

```c#
var config = _container.Resolve<HareDuConfig>();
```

**Microsoft DI**

```c#
var config = _services.GetService<HareDuConfig>();
```

