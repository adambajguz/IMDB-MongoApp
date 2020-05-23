namespace IMDB.Domain.Serilizators
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    public class NullableStringCustomSerializer : SerializerBase<string?>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string? value)
        {
            if (value is string str)
            {
                context.Writer.WriteString(str);
                return;
            }

            context.Writer.WriteNull();
        }

        public override string? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.String)
                return context.Reader.ReadString();

            context.Reader.ReadInt32();
            return null;
        }
    }
}
