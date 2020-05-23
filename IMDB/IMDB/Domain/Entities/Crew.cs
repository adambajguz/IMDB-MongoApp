namespace IMDB.Domain.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using IMDB.Domain.Entities.Base;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Crew : IBaseEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("tconst")]
        public string? TConst { get; set; }

        [BsonElement("directors")]
        public string? Directors { get; set; }

        [BsonElement("writers")]
        public string? Writers { get; set; }

        public IEnumerable<string> GetDirectorsList()
        {
            return Directors?.Split(',').Select(x => x.Trim()) ?? new string[0];
        }

        public IEnumerable<string> GetWritersList()
        {
            return Directors?.Split(',').Select(x => x.Trim()) ?? new string[0];
        }
    }
}
