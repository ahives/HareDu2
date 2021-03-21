namespace HareDu.Extensions
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Core.Extensions;

    public static class JsonExtensions
    {
        // /// <summary>
        // /// Takes an object and returns the JSON text representation of said object.
        // /// </summary>
        // /// <param name="obj"></param>
        // /// <typeparam name="T"></typeparam>
        // /// <returns></returns>
        // public static string ToJsonString<T>(this T obj)
        // {
        //     if (obj.IsNull())
        //         return string.Empty;
        //     
        //     var encoding = new UTF8Encoding(false, true);
        //     
        //     using (var stream = new MemoryStream())
        //     using (var writer = new StreamWriter(stream, encoding, 1024, true))
        //     using (var jsonWriter = new JsonTextWriter(writer))
        //     {
        //         jsonWriter.Formatting = Formatting.Indented;
        //
        //         SerializerCache.Serializer.Serialize(jsonWriter, obj, typeof(T));
        //
        //         jsonWriter.Flush();
        //         writer.Flush();
        //
        //         return encoding.GetString(stream.ToArray());
        //     }
        // }
        //
        // /// <summary>
        // /// Deserializes the contents of <see cref="HttpResponseMessage"/> and returns <see cref="Task{T}"/>
        // /// </summary>
        // /// <param name="responseMessage"></param>
        // /// <typeparam name="T"></typeparam>
        // /// <returns></returns>
        // public static async Task<T> ToObject<T>(this HttpResponseMessage responseMessage)
        // {
        //     string rawResponse = await responseMessage.Content.ReadAsStringAsync();
        //
        //     if (string.IsNullOrWhiteSpace(rawResponse))
        //         return default;
        //     
        //     using (var reader = new StringReader(rawResponse))
        //     using (var jsonReader = new JsonTextReader(reader))
        //     {
        //         T response = SerializerCache.Deserializer.Deserialize<T>(jsonReader);
        //         
        //         return response;
        //     }
        // }
        //
        // /// <summary>
        // /// Deserializes the contents of a string encoded object and returns <see cref="T"/>
        // /// </summary>
        // /// <param name="value"></param>
        // /// <typeparam name="T"></typeparam>
        // /// <returns></returns>
        // public static T ToObject<T>(this string value)
        // {
        //     if (string.IsNullOrWhiteSpace(value))
        //         return default;
        //     
        //     using (var reader = new StringReader(value))
        //     using (var jsonReader = new JsonTextReader(reader))
        //     {
        //         T obj = SerializerCache.Deserializer.Deserialize<T>(jsonReader);
        //         
        //         return obj;
        //     }
        // }

        /// <summary>
        /// Takes an object and returns the JSON text representation of said object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ToJsonString<T>(this T obj, JsonSerializerOptions options)
        {
            if (obj.IsNull())
                return string.Empty;

            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Deserializes the contents of <see cref="HttpResponseMessage"/> and returns <see cref="Task{T}"/>
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> ToObject<T>(this HttpResponseMessage responseMessage, JsonSerializerOptions options)
        {
            string rawResponse = await responseMessage.Content.ReadAsStringAsync();

            return string.IsNullOrWhiteSpace(rawResponse)
                ? default
                : JsonSerializer.Deserialize<T>(rawResponse, options);
        }

        /// <summary>
        /// Deserializes the contents of a string encoded object and returns <see cref="T"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObject<T>(this string value, JsonSerializerOptions options) =>
            string.IsNullOrWhiteSpace(value)
                ? default
                : JsonSerializer.Deserialize<T>(value, options);
    }
}