# Create User

The Broker API allows you to create a user on the RabbitMQ broker. To do so is pretty simple with HareDu 2. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<User>()
    .Create(x =>
    {
        x.Username("user");
        x.Password("password");
        x.PasswordHash("password_hash".ComputePasswordHash());
    });
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<User>()
    .Create(x =>
    {
        x.Username("user");
        x.Password("password");
        x.PasswordHash("password_hash".ComputePasswordHash());
    });
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<User>()
    .Create(x =>
    {
        x.Username("user");
        x.Password("password");
        x.PasswordHash("password_hash".ComputePasswordHash());
    });
```
<br>

By default, the tags on a user is set to "None" but the API allows for setting this property by way of the ```NewUserConfigurator``` configurator using the ```WithTags``` method like so...

```c#
x.WithTags(t =>
{
    t.Administrator();
});
```
<br>

A complete example would look something like this...

```c#
var result = await new BrokerObjectFactory(config)
    .Object<User>()
    .Create(x =>
    {
        x.Username("user");
        x.Password("password");
        x.PasswordHash("password_hash".ComputePasswordHash());
        x.WithTags(t =>
        {
            t.Administrator();
        });
    });
```
<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md).

