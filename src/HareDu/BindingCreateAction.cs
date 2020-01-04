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
namespace HareDu
{
    using System;

    public interface BindingCreateAction
    {
        /// <summary>
        /// Specify the source, destination, and binding type.
        /// </summary>
        /// <param name="definition"></param>
        void Binding(Action<BindingCreateDefinition> definition);
        
        /// <summary>
        /// Specify how the binding should be configured.
        /// </summary>
        /// <param name="configuration"></param>
        void Configure(Action<BindingConfiguration> configuration);

        /// <summary>
        /// Specify the target for which the binding will be created.
        /// </summary>
        /// <param name="target">Define the location where the binding (i.e. virtual host) will be created</param>
        void Target(Action<BindingTarget> target);
    }
}