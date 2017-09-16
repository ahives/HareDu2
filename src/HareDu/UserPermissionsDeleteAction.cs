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

    public interface UserPermissionsDeleteAction
    {
        /// <summary>
        /// Specify the user for which the permission should be deleted.
        /// </summary>
        /// <param name="name">User name</param>
        void User(string name);

        /// <summary>
        /// Specify the target for which the user permission will be deleted.
        /// </summary>
        /// <param name="target">Define what user will for which permissions will be targeted for deletion</param>
        void Targeting(Action<UserPermissionsTarget> target);
    }
}