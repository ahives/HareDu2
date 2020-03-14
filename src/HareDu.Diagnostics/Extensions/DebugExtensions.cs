namespace HareDu.Diagnostics.Extensions
{
    using System;

    public static class DebugExtensions
    {
        public static ScannerResult ScreenDump(this ScannerResult result)
        {
            Console.WriteLine($"Id: {result.Id}");
            Console.WriteLine($"Scanner Id: {result.ScannerId}");
            // Console.WriteLine($"{result}");
            
            for (int i = 0; i < result.Results.Count; i++)
            {
                Console.WriteLine($"Id: {result.Results[i].Id}");
                Console.WriteLine($"Name: {result.Results[i].Name}");
                Console.WriteLine($"Status: {result.Results[i].Status}");
                Console.WriteLine($"Component Id: {result.Results[i].ComponentId}");
                Console.WriteLine($"Component Type: {result.Results[i].ComponentType}");
                Console.WriteLine($"Parent Component Id: {result.Results[i].ParentComponentId}");
                Console.WriteLine();
                // Console.WriteLine($"{result.Results[i]}");
            }

            return result;
        }
    }
}