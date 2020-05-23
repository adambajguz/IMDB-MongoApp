namespace IMDB.Domain.Entities
{
    using IMDB.Domain.Entities.Base;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Rating : IBaseEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("tconst")]
        public string? TConst { get; set; }

        [BsonElement("averageRating")]
        public double AverageRating { get; set; }

        [BsonElement("numVotes")]
        public int NumVotes { get; set; } 
    }
}
