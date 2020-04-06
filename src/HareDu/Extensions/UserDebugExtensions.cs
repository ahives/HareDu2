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
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    public static class UserDebugExtensions
    {
        public static Task<ResultList<UserInfo>> ScreenDump(this Task<ResultList<UserInfo>> result)
        {
            var results = result
                .GetResult()
                .Select(x => x.Data);

            foreach (var item in results)
            {
                Console.WriteLine($"Username: {item.Username}");
                Console.WriteLine($"Password Hash: {item.PasswordHash}");
                Console.WriteLine($"Hashing Algorithm: {item.HashingAlgorithm}");
                Console.WriteLine($"Tags: {item.Tags}");
                Console.WriteLine("-------------------");
                Console.WriteLine();
            }

            return result;
        }
    }
}