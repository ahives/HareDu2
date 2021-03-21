namespace HareDu.Serialization
{
    using System;
    using System.Text.Json.Serialization;

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class JsonInterfaceConverterAttribute :
        JsonConverterAttribute
    {
        public JsonInterfaceConverterAttribute(Type converterType)
            : base(converterType)
        {
        }
    }
}