namespace HareDu.Internal.Serialization
{
//    using System;
//    using System.Linq;
//    using Newtonsoft.Json;
//
//
//    public class MessageDataJsonConverter :
//        JsonConverter
//    {
//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            var messageData = value as IMessageData;
//            if (messageData == null)
//                return;
//
//            var reference = new MessageDataReference {Reference = messageData.Address};
//
//            serializer.Serialize(writer, reference);
//        }
//
//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
//            JsonSerializer serializer)
//        {
//            Type dataType = objectType.GetClosingArguments(typeof(MessageData<>)).First();
//
//            var reference = serializer.Deserialize<MessageDataReference>(reader);
//            if (reference?.Reference == null)
//                return Activator.CreateInstance(typeof(EmptyMessageData<>).MakeGenericType(dataType));
//
//            if (dataType == typeof(string))
//                return new DeserializedMessageData<string>(reference.Reference);
//            if (dataType == typeof(byte[]))
//                return new DeserializedMessageData<byte[]>(reference.Reference);
//
//            throw new MessageDataException("The message data type was unknown: " +
//                                           TypeMetadataCache.GetShortName(dataType));
//        }
//
//        public override bool CanConvert(Type objectType)
//        {
//            return objectType.HasInterface<IMessageData>();
//        }
//    }
}