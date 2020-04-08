# Purging Queues

The Broker API allows you to safely look at (i.e. peek) messages on a queue and put them back. To do so is pretty simple with HareDu 2. You can do it yourself or the IoC way. In either case you need only configure the peek action per your requirements and leave the rest up to the API.

**Do It Yourself**

```csharp
var result = await new BrokerObjectFactory(config)
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Queue("your_queue");
                    x.Configure(c =>
                    {
                        c.Take(5);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>


**Autofac**

```csharp
var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Queue("your_queue");
                    x.Configure(c =>
                    {
                        c.Take(5);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

**.NET Core DI**

```csharp
var result = await _services.GetService<IBrokerObjectFactory>()
                .Object<Queue>()
                .Peek(x =>
                {
                    x.Queue("your_queue");
                    x.Configure(c =>
                    {
                        c.Take(5);
                        c.AckMode(RequeueMode.AckRequeue);
                        c.TruncateIfAbove(5000);
                        c.Encoding(MessageEncoding.Auto);
                    });
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/configuration.md) .

