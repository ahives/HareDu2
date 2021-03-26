# Delete User

The Broker API allows you to delete a user(s) from the RabbitMQ broker. To do so is pretty simple with HareDu 3. You can do it yourself or the DI way.

**Do It Yourself**

```c#
var result = await new BrokerObjectFactory(config)
    .Object<User>()
    .Delete("username");
```
<br>

**Autofac**

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .Object<User>()
    .Delete("username");
```
<br>

**Microsoft DI**

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<User>()
    .Delete("username");
```
<br>

There is an overloaded version of the above method that will allow you to bulk delete users that looks like this...

```c#
var result = await _services.GetService<IBrokerObjectFactory>()
    .Object<User>()
    .Delete(new List<string>{"username1", "username2", "username3"});
```

<br>

The other way to delete a user is to call the extension methods off of ```IBrokerObjectFactory``` like so...

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .DeleteUser("username");
```

...and for deleting in bulk, you can use the following extension method like so...

```c#
var result = await _container.Resolve<IBrokerObjectFactory>()
    .DeleteUsers(new List<string>{"username1", "username2", "username3"});
```

<br>

All examples in this document assumes the broker has been configured. If you want to know how then go to the Configuration documentation [here](https://github.com/ahives/HareDu3/blob/master/docs/configuration.md).

