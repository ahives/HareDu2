namespace HareDu.Extensions
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Core.Extensions;

    public static class JsonExtensions
    {
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