﻿// Copyright 2013-2019 Albert L. Hives
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
namespace HareDu.Core.Extensions
{
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Internal.Serialization;
    using Newtonsoft.Json;

    public static class JsonExtensions
    {
        /// <summary>
        /// Takes an object and returns the JSON text representation of said object.
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ToJsonString<T>(this T obj)
        {
            if (obj.IsNull())
                return string.Empty;
            
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
        
        internal static async Task<T> DeserializeResponse<T>(this HttpResponseMessage responseMessage)
        {
            string rawResponse = await responseMessage.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(rawResponse))
                return default;
            
            using (var reader = new StringReader(rawResponse))
            using (var jsonReader = new JsonTextReader(reader))
            {
                T response = SerializerCache.Deserializer.Deserialize<T>(jsonReader);
                
                return response;
            }
        }
    }
}