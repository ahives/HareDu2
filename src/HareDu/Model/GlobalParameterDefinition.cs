namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;
    using Core.Extensions;

    public class GlobalParameterDefinition
    {
        public GlobalParameterDefinition(string name, IDictionary<string, ArgumentValue<object>> arguments, object argument)
        {
            Name = name;

            if (!argument.IsNull())
            {
                Value = argument;
                return;
            }
                    
            if (arguments.IsNull())
                return;
                    
            Value = arguments.ToDictionary(x => x.Key, x => x.Value.Value);
        }

        public GlobalParameterDefinition()
        {
        }

        [JsonPropertyName("name")]
        public string Name { get; set; }
            
        [JsonPropertyName("value")]
        public object Value { get; set; }
    }
}