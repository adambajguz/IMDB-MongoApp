namespace IMDB.Domain.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using IMDB.Domain.Entities.Base;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Name : IBaseEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("nconst")]
        public string? NConst { get; set; }

        [BsonElement("primaryName")]
        public string? PrimaryName { get; set; }

        [BsonElement("birthYear")]
        public int BirthYear { get; set; }

        [BsonElement("deathYear")]
        public int DeathYear { get; set; }

        [BsonElement("primaryProfession")]
        public string? PrimaryProfession { get; set; }

        [BsonElement("knownForTitles")]
        public string? KnownForTitles { get; set; }

        public IEnumerable<string> GetPrimaryProfessionsList()
        {
            return PrimaryProfession?.Split(',').Select(x => x.Trim()) ?? new string[0];
        }

        public IEnumerable<string> GetKnownForTitlesList()
        {
            return KnownForTitles?.Split(',').Select(x => x.Trim()) ?? new string[0];
        }
    }
}
