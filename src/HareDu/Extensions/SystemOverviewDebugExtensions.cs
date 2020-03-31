namespace HareDu.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class SystemOverviewDebugExtensions
    {
        public static Task<Result<SystemOverviewInfo>> ScreenDump(this Task<Result<SystemOverviewInfo>> result)
        {
            var item = result
                .GetResult()
                .Select(x => x.Data);
            
            Console.WriteLine($"Node: {item.Node}");
            Console.WriteLine($"Management Version: {item.ManagementVersion}");
            Console.WriteLine($"Cluster Name: {item.ClusterName}");
            Console.WriteLine($"Erlang Verion: {item.ErlangVersion}");
            Console.WriteLine($"Erlang Verion (Full): {item.ErlangFullVersion}");
            Console.WriteLine($"RMQ Version: {item.RabbitMqVersion}");
            Console.WriteLine($"Rates Mode: {item.RatesMode}");
            Console.WriteLine($"Total Disk Reads: {item.MessageStats?.TotalDiskReads}");
            Console.WriteLine($"Node: {item.StatsDatabaseEventQueue}");
            Console.WriteLine();

            foreach (var listener in item.Listeners)
            {
                Console.WriteLine($"\tNode: {listener.Node}");
                Console.WriteLine($"\tPort: {listener.Port}");
                Console.WriteLine($"\tProtocol: {listener.Protocol}");
                Console.WriteLine($"\tIP Address: {listener.IPAddress}");

                Console.WriteLine("Socket Options");
                foreach (var option in listener.SocketOptions)
                {
                    Console.WriteLine($"\t\tBacklog: {option.Backlog}");
                    Console.WriteLine($"\t\tNo Delay: {option.NoDelay}");
                    Console.WriteLine($"\t\tExit on Close: {option.ExitOnClose}");
                }
                Console.WriteLine("\t-------------------");
            }
            Console.WriteLine($"Node: {item.Listeners}");
            
            Console.WriteLine("Totals");
            Console.WriteLine($"\tChannels: {item.ObjectTotals?.TotalChannels}");
            Console.WriteLine($"\tConnections: {item.ObjectTotals?.TotalConnections}");
            Console.WriteLine($"\tConsumers: {item.ObjectTotals?.TotalConsumers}");
            Console.WriteLine($"\tExchanges: {item.ObjectTotals?.TotalExchanges}");
            Console.WriteLine($"\tQueues: {item.ObjectTotals?.TotalQueues}");
            Console.WriteLine();
            
            Console.WriteLine("Churn Rates");
            Console.WriteLine("\tChannels");
            Console.WriteLine($"\t\tCreated: {item.ChurnRates?.TotalChannelsCreated} (total), {item.ChurnRates?.CreatedChannelDetails?.Rate} (rate)");
            Console.WriteLine($"\t\tClosed: {item.ChurnRates?.TotalChannelsClosed} (total), {item.ChurnRates?.ClosedChannelDetails?.Rate} (rate)");
            Console.WriteLine("\tConnections");
            Console.WriteLine($"\t\tCreated: {item.ChurnRates?.TotalConnectionsCreated} (total), {item.ChurnRates?.CreatedConnectionDetails?.Rate} (rate)");
            Console.WriteLine($"\t\tClosed: {item.ChurnRates?.TotalConnectionsClosed} (total), {item.ChurnRates?.ClosedConnectionDetails?.Rate} (rate)");
            Console.WriteLine("\tQueues");
            Console.WriteLine($"\t\tCreated: {item.ChurnRates?.TotalQueuesCreated} (total), {item.ChurnRates?.CreatedQueueDetails?.Rate} (rate)");
            Console.WriteLine($"\t\tDeclared: {item.ChurnRates?.TotalQueuesDeclared} (total), {item.ChurnRates?.DeclaredQueueDetails?.Rate} (rate)");
            Console.WriteLine($"\t\tDeleted: {item.ChurnRates?.TotalQueuesDeleted} (total), {item.ChurnRates?.DeletedQueueDetails?.Rate} (rate)");
            Console.WriteLine();
            
            Console.WriteLine("Queue Stats");
            Console.WriteLine($"\tMessages: {item.QueueStats?.TotalMessages} (total), {item.QueueStats?.MessageDetails} (rate)");
            Console.WriteLine($"\tUnacknowledged Delivered: {item.QueueStats?.TotalUnacknowledgedDeliveredMessages} (total), {item.QueueStats?.UnacknowledgedDeliveredMessagesDetails?.Rate} (rate)");
            Console.WriteLine($"\tReady: {item.QueueStats?.TotalMessagesReadyForDelivery} (total), {item.QueueStats?.MessagesReadyForDeliveryDetails?.Rate} (rate)");
            Console.WriteLine();

            Console.WriteLine("Listeners");
            foreach (var listener in item.Listeners)
            {
                Console.WriteLine($"\tNode: {listener.Node}");
                Console.WriteLine($"\tPort: {listener.Port}");
                Console.WriteLine($"\tProtocol: {listener.Protocol}");
                Console.WriteLine($"\tIP Address: {listener.IPAddress}");

                Console.WriteLine("\tSocket Options");
                foreach (var option in listener.SocketOptions)
                {
                    Console.WriteLine("\t\t-------------------");
                    Console.WriteLine($"\t\tBacklog: {option.Backlog}");
                    Console.WriteLine($"\t\tNo Delay: {option.NoDelay}");
                    Console.WriteLine($"\t\tExit on Close: {option.ExitOnClose}");
                }
                
                Console.WriteLine("\t-------------------");
            }

            Console.WriteLine("Contexts");
            foreach (var context in item.Contexts)
            {
                Console.WriteLine($"\tDescription: {context.Description}");
                Console.WriteLine($"\tPort: {context.Port}");
                Console.WriteLine($"\tPath: {context.Path}");
                Console.WriteLine("\t-------------------");
            }
            
            Console.WriteLine("****************************************************");
            Console.WriteLine();

            return result;
        }
    }
}