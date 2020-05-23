namespace IMDB.Domain.Serilizators
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    public class NullableIntCustomSerializer : SerializerBase<int?>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, int? value)
        {
            if (value is int i)
            {
                context.Writer.WriteInt32(i);
                return;
            }

            context.Writer.WriteNull();
        }

        public override int? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.Int32)
            {
                context.Reader.ReadInt32();
                return null;
            }

            string str = context.Reader.ReadString();

            return str == @"\N" || str == "null" ? (int?)null : int.Parse(str);
        }
    }
}
