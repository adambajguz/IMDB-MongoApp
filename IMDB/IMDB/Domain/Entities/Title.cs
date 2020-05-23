namespace IMDB.Domain.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using IMDB.Domain.Entities.Base;
    using IMDB.Domain.Serilizators;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Title : IBaseEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("tconst")]
        public string? TConst { get; set; }

        [BsonElement("titleType")]
        [BsonSerializer(typeof(NullableStringCustomSerializer))]
        public string? TitleType { get; set; }

        [BsonElement("primaryTitle")]
        [BsonSerializer(typeof(NullableStringCustomSerializer))]
        public string? PrimaryTitle { get; set; }

        [BsonElement("originalTitle")]
        [BsonSerializer(typeof(NullableStringCustomSerializer))]
        public string? OriginalTitle { get; set; }

        [BsonElement("isAdult")]
        public int IsAdult { get; set; }

        [BsonElement("startYear")]
        public int StartYear { get; set; }

        [BsonElement("endYear")]
        public int EndYear { get; set; }

        [BsonElement("runtimeMinutes")]
        public int RuntimeMinutes { get; set; }

        [BsonElement("genres")]
        public string? Genres { get; set; }

        [BsonElement("max")]
        public int Max { get; set; }

        public IEnumerable<string> GetGenresList()
        {
            return Genres?.Split(',').Select(x => x.Trim()) ?? new string[0];
        }
    }
}
