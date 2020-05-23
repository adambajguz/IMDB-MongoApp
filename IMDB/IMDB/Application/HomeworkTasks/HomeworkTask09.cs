namespace IMDB.Application.HomeworkTasks
{
    using System.Linq;
    using System.Threading.Tasks;
    using IMDB.Application.Interfaces;
    using IMDB.Domain.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    [HomeworkExclude]
    public sealed class HomeworkTask09 : IHomeworkTask
    {
        private readonly IDatabaseContext _databaseContext;

        public HomeworkTask09(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<object> RunAsync()
        {
            IMongoQueryable<Name> queryable = _databaseContext.Names.AsQueryable();

            await _databaseContext.Names.Indexes.DropOneAsync("primaryName_text");

            CreateIndexModel<Name> indexModel = new CreateIndexModel<Name>(Builders<Name>.IndexKeys.Text(x => x.PrimaryProfession));
            await _databaseContext.Names.Indexes.CreateOneAsync(indexModel);

            IAsyncCursor<MongoDB.Bson.BsonDocument> indexes = await _databaseContext.Names.Indexes.ListAsync();

            var partialQuery = queryable.Where(x => x.BirthYear >= 1950 && x.BirthYear <= 1980 &&
                                                    (x.PrimaryProfession.Contains("actor") ||
                                                     x.PrimaryProfession.Contains("director")));

            var list = await partialQuery.Take(10)
                                         .Select((x) => new { x.PrimaryName, x.BirthYear, x.PrimaryProfession })
                                         .ToListAsync();

            int count = await partialQuery.CountAsync();

            Result result = new Result
            {
                Indexes = indexes.ToEnumerable(),
                People = list,
                Count = count
            };
            return result;
        }

        public sealed class Result
        {
            public object? Indexes { get; set; }
            public int Count { get; set; }
            public object? People { get; set; }
        }
    }
}
