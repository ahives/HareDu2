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

    public interface PolicyDeleteAction
    {
        /// <summary>
        /// Specify the name of the policy.
        /// </summary>
        /// <param name="name"></param>
        void Policy(string name);

        /// <summary>
        /// Specify what virtual host will the policy be deleted.
        /// </summary>
        /// <param name="target">Define where the policy will be delete from</param>
        void Target(Action<PolicyTarget> target);
    }
}