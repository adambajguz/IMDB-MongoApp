namespace IMDB.Domain.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using IMDB.Domain.Entities.Base;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Cast : IBaseEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("tconst")]
        public string? TConst { get; set; }

        [BsonElement("ordering")]
        public int Ordering { get; set; }

        [BsonElement("nconst")]
        public string? NConst { get; set; }

        [BsonElement("category")]
        public string? Category { get; set; }

        [BsonElement("job")]
        public string? Job { get; set; }

        [BsonElement("characters")]
        public string? Characters { get; set; }

        public IEnumerable<string> GetCategoriesList()
        {
            return Category?.Split('\t').Select(x => x.Trim().Replace(@"\N", "")) ?? new string[0];
        }
    }
}
