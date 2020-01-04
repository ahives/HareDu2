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

    public interface UserCreateAction
    {
        /// <summary>
        /// Specify the login username for the corresponding user.
        /// </summary>
        /// <param name="username"></param>
        void Username(string username);
        
        /// <summary>
        /// Specify the login password for the corresponding user.
        /// </summary>
        /// <param name="password"></param>
        void Password(string password);
        
        /// <summary>
        /// Specify the password for which a hash will eventually be computed.
        /// </summary>
        /// <param name="passwordHash"></param>
        void PasswordHash(string passwordHash);
        
        /// <summary>
        /// Specify the type of access the corresponding user has.
        /// </summary>
        /// <param name="tags"></param>
        void WithTags(Action<UserAccessOptions> tags);
    }
}