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
namespace HareDu
{
    using System;

    public interface TopicPermissionsCreateAction
    {
        /// <summary>
        /// Specify the user for which the permission should be assigned to.
        /// </summary>
        /// <param name="username"></param>
        void User(string username);

        /// <summary>
        /// Define how the user permission is to be created.
        /// </summary>
        /// <param name="definition"></param>
        void Configure(Action<TopicPermissionsConfiguration> configure);

        /// <summary>
        /// Specify the target for which the user permission will be created.
        /// </summary>
        /// <param name="target">Define which user is associated with the permissions that are being created.</param>
        void VirtualHost(string name);
    }
}