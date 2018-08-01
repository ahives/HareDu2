// Copyright 2013-2018 Albert L. Hives
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
namespace HareDu
{
    using System;

    public interface ExchangeCreateAction
    {
        /// <summary>
        /// Specify the name of the exchange.
        /// </summary>
        /// <param name="name">RabbitMQ exchange name</param>
        void Exchange(string name);

        /// <summary>
        /// Specify how should the exchange be configured.
        /// </summary>
        /// <param name="configuration">User-defined configuration</param>
        void Configure(Action<ExchangeConfiguration> configuration);

        /// <summary>
        /// Specify the target for which the exchange will be created.
        /// </summary>
        /// <param name="target">Define the location of the exchange (i.e. virtual host) that is targeted for deletion</param>
        void Target(Action<ExchangeTarget> target);
    }
}