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
namespace HareDu.Core
{
    public interface TopicPermissionsConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        void OnExchange(string name);

        /// <summary>
        /// Specify the pattern of what types of writes are allowable for this permission.
        /// </summary>
        /// <param name="pattern"></param>
        void UsingWritePattern(string pattern);

        /// <summary>
        /// Specify the pattern of what types of reads are allowable for this permission.
        /// </summary>
        /// <param name="pattern"></param>
        void UsingReadPattern(string pattern);
    }
}