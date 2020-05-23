namespace IMDB.Infrastructure.Persistence
{
    using IMDB.Application.Interfaces;
    using IMDB.Domain.Entities;
    using MongoDB.Bson.IO;
    using MongoDB.Driver;

    public class DatabaseContext : IDatabaseContext
    {
        private readonly JsonWriterSettings _jsonSettings;

        public MongoClient DbClient { get; }
        public IMongoDatabase Db { get; }

        public DatabaseContext(string connectionString, string databaseName)
        {
            _jsonSettings = new JsonWriterSettings { Indent = true };

            MongoClientSettings mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);
            mongoClientSettings.ClusterConfigurator = cfg =>
            {
                cfg.Subscribe(new LogMongoDBEvents(_jsonSettings));

                //cfg.Subscribe<CommandStartedEvent>(e =>
                //{
                //    Log.Debug($"{e.CommandName} - {e.Command.ToJson(writerSettings: _jsonSettings)}");
                //});
            };

            DbClient = new MongoClient(mongoClientSettings);
            Db = DbClient.GetDatabase(databaseName);
        }

        public IMongoCollection<Cast> Casts => Db.GetCollection<Cast>("Cast");
        public IMongoCollection<Crew> Crews => Db.GetCollection<Crew>("Crew");
        public IMongoCollection<Name> Names => Db.GetCollection<Name>("Name");
        public IMongoCollection<Rating> Ratings => Db.GetCollection<Rating>("Rating");
        public IMongoCollection<Title> Titles => Db.GetCollection<Title>("Title");
    }
}

