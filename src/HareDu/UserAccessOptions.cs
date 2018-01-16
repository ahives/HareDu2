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
    public interface UserAccessOptions
    {
        /// <summary>
        /// Specify that the corresponding user has no associated priviledges.
        /// </summary>
        void None();
        
        /// <summary>
        /// Specify that the corresponding user has administrative priviledges.
        /// </summary>
        void Administrator();
        
        /// <summary>
        /// Specify that the corresponding user has monitoring pviliedges.
        /// </summary>
        void Monitoring();
        
        /// <summary>
        /// Specify that the corresponding user has management priviledges.
        /// </summary>
        void Management();
    }
}