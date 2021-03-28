namespace HareDu.Extensions
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Serialization;

    public static class JsonExtensions
    {
        /// <summary>
        /// Takes an object and returns the JSON text representation of said object using the specified serialization options.
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
        /// Takes an object and returns the JSON text representation of said object using default serialization options.
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ToJsonString<T>(this T obj)
        {
            if (obj.IsNull())
                return string.Empty;

            return JsonSerializer.Serialize(obj, Deserializer.Options);
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
        /// Deserializes the contents of <see cref="HttpResponseMessage"/> and returns <see cref="Task{T}"/> using default deserialization options.
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> ToObject<T>(this HttpResponseMessage responseMessage)
        {
            string rawResponse = await responseMessage.Content.ReadAsStringAsync();

            return string.IsNullOrWhiteSpace(rawResponse)
                ? default
                : JsonSerializer.Deserialize<T>(rawResponse, Deserializer.Options);
        }

        /// <summary>
        /// Deserializes the contents of a string encoded object and returns <see cref="T"/> given the specified deserialization options.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObject<T>(this string value, JsonSerializerOptions options) =>
            string.IsNullOrWhiteSpace(value)
                ? default
                : JsonSerializer.Deserialize<T>(value, options);

        /// <summary>
        /// Deserializes the contents of a string encoded object and returns <see cref="T"/> using default deserialization options.
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObject<T>(this string value) =>
            string.IsNullOrWhiteSpace(value)
                ? default
                : JsonSerializer.Deserialize<T>(value, Deserializer.Options);
    }
}