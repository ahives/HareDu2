# Changelist

| Version | | Description | Type | Breaking Change? |
| --- | --- | --- | --- | --- |
| **2.2.0** | 1 | Fixed issue with create exchange not serializing request properly for type property | Bug Fix | No |
| | 2 | Changed AppliedTo property on OperatorPolicyInfo to be a enumeration | Enhancement | Yes |
| | 3 | Fixed issue with shovel request not serializing correctly so that shovels are not being created correctly | Bug Fix | No |
| | 4 | Removed JSON.NET dependency and replaced with System.Text.Json parser introduced in C# 8 | Enhancement | No |
| | 5 | Introduced new object in Broker API called Shovel for creating, viewing, and deleting dynamic shovels that are used to move messages between exchanges and queues | Enhancement | No |
| | 6 | Changed method signatures for Broker API objects to make them less verbose | Enhancement | Yes |
| | 7 | Introduced extension methods for Broker API objects to call methods directly off of IBrokerObjectFactory | Enhancement | No |
| | 8 | Added support for configuring HareDu APIs in JSON | Enhancement | No |
| | 9 | Deprecated Peek method on Queue object in Broker API | Deprecated | Yes |
| | 10 | Changed SystemOverview object in Broker API to BrokerSystem | Enhancement | Yes |
| | 11 | Introduced new object in Broker API called OperatorPolicy | New | No |
| | 12 | Introduced new method overload on User object in Broker API called Delete and DeleteUsers extension method to perform bulk delete of users | New | No |
| | 13 | Introduced new method on Connection object in Broker API called Delete and DeleteConnection extension method to delete a connection to the RabbitMQ broker | New | No |
| | 14 | Deprecated method overloads for RegisterObserver and RegisterObservers in IScannerFactory | Deprecated | Yes |
| | 15 | Deprecated metadata properties Id, Name, and Description implemented by diagnostic probes | Deprecated | Yes |
| | 16 | Added new metadata property called DiagnosticProbeMetadata on all diagnostic probes that encapsulates miscellaneous information pertinent to putting the specified probe in the proper context | New | No |
| | 17 | Changed method names of RegisterScanner and RegisterProbe to TryRegisterScanner and TryRegisterProbe, respectively, in IScannerFactory | Enhancement | Yes |
| | 18 | Added more developer documentation (e.g., API intellisense) | Enhancement | No |
| | | | | |
| **2.2.1** | 1 | Fixed issue with QueueInfo.BackingQueueStatus.TargetTotalMessagesInRAM property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | 2 | Fixed issue with QueueInfo.BackingQueueStatus.BackingQueueMode property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | 3 | Fixed issue with QueueInfo.State property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | 4 | Fixed issue with ChannelInfo.State property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | 5 | Fixed issue with NodeInfo.RatesMode property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | 6 | Fixed issue with ConnectionInfo.Type property not being deserialized correctly from the RabbitMQ HTTP queues API | Bug Fix | Yes |
| | | | | |
| **2.2.2** | 1 | Deprecated metadata properties Identifier implemented by diagnostic scanners | Deprecated | Yes |
| | 2 | Added new metadata property called DiagnosticScannerMetadata on all diagnostic scanners that encapsulates miscellaneous information pertinent to putting the specified scanner into the proper context | New | No |
| | | | | |
| **2.2.3** | 1 | Fixed issue with configuration serialization | Bug Fix | No |
| | 2 | Marked API methods that are obsolete in developer documentation | New | No |
