# Delete Binding

The Broker API allows you to delete a binding from a RabbitMQ broker object (e.g., exchanges and/or queues). To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<Binding>()
    .Delete(x =>
    {
        x.Binding(b =>
        {
            b.Name("binding_name");
            b.Source("source_exchange");
            b.Destination("destination_exchange");
            b.Type(BindingType.Exchange);
        });
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<Binding>()
    .Delete(x =>
    {
        x.Binding(b =>
        {
            b.Name("binding_name");
            b.Source("source_exchange");
            b.Destination("destination_exchange");
            b.Type(BindingType.Exchange);
        });
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<Binding>()
    .Delete(x =>
    {
        x.Binding(b =>
        {
            b.Name("binding_name");
            b.Source("source_exchange");
            b.Destination("destination_exchange");
            b.Type(BindingType.Exchange);
        });
        x.Targeting(t => t.VirtualHost("vhost"));
    });
```
<br>

If the destination binding is a queue then you simply need to set the ```Type``` accordingly and specify the queue's name via the ```Destination``` method like so...

```c#
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

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md).

