// Copyright 2013-2020 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class ExchangeDebugExtensions
    {
        public static Task<ResultList<ExchangeInfo>> ScreenDump(this Task<ResultList<ExchangeInfo>> result)
        {
            var results = result
                .GetResult()
                .Select(x => x.Data);
            
            foreach (var item in results)
            {
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Virtual Host: {item.VirtualHost}");
                Console.WriteLine($"Auto Delete: {item.AutoDelete}");
                Console.WriteLine($"Internal: {item.Internal}");
                Console.WriteLine($"Durable: {item.Durable}");
                Console.WriteLine($"Routing Type: {item.RoutingType}");
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

            return result;
        }

        public static ResultList<ExchangeInfo> ScreenDump(this ResultList<ExchangeInfo> result)
        {
            var results = result
                .Select(x => x.Data);
            
            foreach (var item in results)
            {
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Virtual Host: {item.VirtualHost}");
                Console.WriteLine($"Auto Delete: {item.AutoDelete}");
                Console.WriteLine($"Internal: {item.Internal}");
                Console.WriteLine($"Durable: {item.Durable}");
                Console.WriteLine($"Routing Type: {item.RoutingType}");
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

            return result;
        }

        public static IReadOnlyList<ExchangeInfo> ScreenDump(this IReadOnlyList<ExchangeInfo> result)
        {
            foreach (var item in result)
            {
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Virtual Host: {item.VirtualHost}");
                Console.WriteLine($"Auto Delete: {item.AutoDelete}");
                Console.WriteLine($"Internal: {item.Internal}");
                Console.WriteLine($"Durable: {item.Durable}");
                Console.WriteLine($"Routing Type: {item.RoutingType}");
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }

            return result;
        }
    }
}