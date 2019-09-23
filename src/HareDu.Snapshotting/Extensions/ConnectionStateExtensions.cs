// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Snapshotting.Extensions
{
    using Model;

    public static class ConnectionStateExtensions
    {
        public static ConnectionState Convert(this string value)
        {
            switch (value)
            {
                case "starting":
                    return ConnectionState.Starting;
                
                case "tuning":
                    return ConnectionState.Tuning;
                
                case "opening":
                    return ConnectionState.Opening;
                
                case "flow":
                    return ConnectionState.Flow;
                
                case "blocking":
                    return ConnectionState.Blocking;
                
                case "blocked":
                    return ConnectionState.Blocked;
                
                case "closing":
                    return ConnectionState.Closing;
                
                case "closed":
                    return ConnectionState.Closed;
                
                case "running":
                    return ConnectionState.Running;

                default:
                    return ConnectionState.Inconclusive;
            }
        }
    }
}