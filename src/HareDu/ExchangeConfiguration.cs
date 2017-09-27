// Copyright 2013-2017 Albert L. Hives
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

    public interface ExchangeConfiguration
    {
        /// <summary>
        /// Specify the message routing type (e.g. fanout, direct, topic, etc.).
        /// </summary>
        /// <param name="routingType"></param>
        void HasRoutingType(ExchangeRoutingType routingType);

        /// <summary>
        /// Specify that the exchange is durable.
        /// </summary>
        void IsDurable();

        /// <summary>
        /// Specify that the exchange is for internal use only.
        /// </summary>
        void IsForInternalUse();

        /// <summary>
        /// Specify user-defined arguments used to configure the exchange.
        /// </summary>
        /// <param name="arguments"></param>
        void HasArguments(Action<ExchangeDefinitionArguments> arguments);

        /// <summary>
        /// Specify that the exchange will be deleted when there are no consumers.
        /// </summary>
        void AutoDeleteWhenNotInUse();
    }
}