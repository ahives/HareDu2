# HareDu 2

HareDu 2 comes with four major APIs; that is, Broker, Snapshot, Diagnostics, and Configuration, respectively.

<br>

**Note: if you are using HareDu 1.2 and want the latest updates, HareDu 2 now ships with bindings for .NET Framework 4.6.2 (minimum) with HareDu 2.2.4. There will be some breaking changes but its well worth it. If possible, upgrade to HareDu 3, which supports .NET 5, natively.**

### HareDu 2 NuGet Packages

| Package Name |  | .NET Runtime |
|---| --- | --- |
| **API** |  |  |
| [HareDu.Core](https://www.nuget.org/packages/HareDu.Core/) | Configuration API | Core 2.0, Framework 4.6.2 |
| [HareDu](https://www.nuget.org/packages/HareDu/) | Broker API | Core 2.0, Framework 4.6.2 |
| [HareDu.Snapshotting](https://www.nuget.org/packages/HareDu.Snapshotting/) | Snapshot API | Core 2.0, Framework 4.6.2 |
| [HareDu.Diagnostics](https://www.nuget.org/packages/HareDu.Diagnostics/) | Diagnostics API | Core 2.0, Framework 4.6.2 |
| **Dependency Injection Containers** | | |
| [HareDu.AutofacIntegration](https://www.nuget.org/packages/HareDu.AutofacIntegration/) | Autofac Integration API | Core 2.0, Framework 4.6.2 |
| [HareDu.CoreIntegration](https://www.nuget.org/packages/HareDu.CoreIntegration/) | Microsoft DI Integration API| Core 2.0, Framework 4.6.2 |

<br>

### Fundamentals
To use HareDu, you must have the appropriate RabbitMQ plugins installed and enabled.

| API |  |  |
| --- | --- | --- |
| **Broker** | [Current](https://github.com/ahives/HareDu2/blob/master/docs/broker-api.md) | [Deprecated](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/broker-api.md) |
| **Snapshot** | [Current](https://github.com/ahives/HareDu2/blob/master/docs/snapshot-api.md) | [Deprecated](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/snapshot-api.md) |
| **Diagnostics** | [Current](https://github.com/ahives/HareDu2/blob/master/docs/diagnostics-api.md) | [Deprecated](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/diagnostics-api.md) |
| **Configuration** | [Current](https://github.com/ahives/HareDu2/blob/master/docs/configuration.md) | [Deprecated](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/configuration.md) |
| **Autofac Integration** | [Current](https://github.com/ahives/HareDu2/blob/master/docs/autofac-integration.md) | [Deprecated](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/autofac-integration.md) |
| **.NET Core Integration** | [Current](https://github.com/ahives/HareDu2/blob/master/docs/core-integration.md) | [Deprecated](https://github.com/ahives/HareDu2/blob/master/docs/deprecated/core-integration.md) |
