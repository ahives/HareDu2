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

    public interface PolicyConfiguration
    {
        /// <summary>
        /// Specify the pattern which the policy will be applied.
        /// </summary>
        /// <param name="pattern"></param>
        void UsingPattern(string pattern);
        
        /// <summary>
        /// Specify arguments for the policy.
        /// </summary>
        /// <param name="arguments">Pre-defined arguments applied to the definition of the policy.</param>
        void HasArguments(Action<PolicyDefinitionArguments> arguments);
        
        /// <summary>
        /// Specify the priority for which the policy will be executed.
        /// </summary>
        /// <param name="priority"></param>
        void HasPriority(int priority);
        
        /// <summary>
        /// Specify how the policy is applied.
        /// </summary>
        /// <param name="applyTo"></param>
        void AppliedTo(string applyTo);
    }
}