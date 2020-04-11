# Deleting Bindings

The Broker API allows you to delete a binding from a RabbitMQ broker object (e.g., exchanges and/or queues). To do so is pretty simple with HareDu 2. You can do it yourself or the IoC way.

**Do It Yourself**

```csharp
var result = await new BrokerObjectFactory(config)
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name("your_binding_name");
                        b.Source("your_exchange");
                        b.Destination("your_exchange");
                        b.Type(BindingType.Exchange);
                    });
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

**Autofac**

```csharp
var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name("your_binding_name");
                        b.Source("your_exchange");
                        b.Destination("your_exchange");
                        b.Type(BindingType.Exchange);
                    });
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

**.NET Core DI**

```csharp
var result = await _services.GetService<IBrokerObjectFactory>()
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name("your_binding_name");
                        b.Source("your_exchange");
                        b.Destination("your_exchange");
                        b.Type(BindingType.Exchange);
                    });
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

If the destination binding is a queue then you simply need to set the ```Type``` accordingly and specify the queue's name via the ```Destination``` method like so...

```csharp
var result = await new BrokerObjectFactory(config)
                .Object<Binding>()
                .Delete(x =>
                {
                    x.Binding(b =>
                    {
                        b.Name("your_binding_name");
                        b.Source("your_exchange");
                        b.Destination("your_queue");
                        b.Type(BindingType.Queue);
                    });
                    x.Targeting(t => t.VirtualHost("your_vhost"));
                });
```
<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/configuration.md) .

