# Autofac Integration

For those that are interested in quickly building applications with HareDu 3, the best way to do that is by using one of the supported Dependency Injection integration packages. In this section we will go over how to use the Autofac Integration package in your applications.

Before you can use any of the APIs, you must configure HareDu by calling ```AddHareDu``` like so...

```c#
var container = new ContainerBuilder()
                .AddHareDu(x =>
                {
                    x.Broker(b =>
                    {
                        b.ConnectTo("http://localhost:15672");
                        b.UsingCredentials("guest", "guest");
                    });
                    x.Diagnostics(d =>
                    {
                        d.Probes(p =>
                        {
                            p.SetConsumerUtilizationThreshold(0.5M);
                                ...
                        });
                    });
                })
                .Build();
```

...or if you have an appsettings.json file with a configuration section named HareDuConfig then it is...
```c#
var container = new ContainerBuilder()
                .AddHareDu()
                .Build();
```
<br>

If your settings file and/or configuration section within said file is different, then, you need only supply the appropriate values when calling ```AddHareDu``` like so...
```c#
var container = new ContainerBuilder()
                .AddHareDu("mysettings.json", "MyConfigSection")
                .Build();
```
