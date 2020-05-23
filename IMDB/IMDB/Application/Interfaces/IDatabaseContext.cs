namespace IMDB.Application.Interfaces
{
    using IMDB.Domain.Entities;
    using MongoDB.Driver;

    public interface IDatabaseContext
    {
        IMongoCollection<Cast> Casts { get; }
        IMongoCollection<Crew> Crews { get; }
        IMongoDatabase Db { get; }
        MongoClient DbClient { get; }
        IMongoCollection<Name> Names { get; }
        IMongoCollection<Rating> Ratings { get; }
        IMongoCollection<Title> Titles { get; }
    }
}