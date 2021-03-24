namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class BulkUserDeleteRequest
    {
        public BulkUserDeleteRequest(IList<string> users)
        {
            Users = users;
        }

        public BulkUserDeleteRequest()
        {
        }

        [JsonPropertyName("users")]
        public IList<string> Users { get; set; }
    }
}