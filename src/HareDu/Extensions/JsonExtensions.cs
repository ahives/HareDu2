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
namespace HareDu.Extensions
{
    using System.IO;
    using System.Text;
    using Internal.Serialization;
    using Newtonsoft.Json;

    public static class JsonExtensions
    {
        public static string ToJsonString<T>(this T obj)
        {
            var encoding = new UTF8Encoding(false, true);
            
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, encoding, 1024, true))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                jsonWriter.Formatting = Formatting.Indented;

                SerializerCache.Serializer.Serialize(jsonWriter, obj, typeof(T));

                jsonWriter.Flush();
                writer.Flush();

                return encoding.GetString(stream.ToArray());
            }
        }
    }
}