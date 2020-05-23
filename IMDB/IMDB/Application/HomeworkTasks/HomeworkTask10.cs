namespace IMDB.Application.HomeworkTasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using IMDB.Application.Interfaces;
    using IMDB.Domain.Entities;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    [HomeworkExclude]
    public sealed class HomeworkTask10 : IHomeworkTask
    {
        private readonly IDatabaseContext _databaseContext;

        public HomeworkTask10(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<object> RunAsync()
        {
            IMongoQueryable<Name> queryable = _databaseContext.Names.AsQueryable();

            //await _databaseContext.Names.Indexes.DropOneAsync("primaryProfession_text");

            CreateIndexModel<Name> indexModel = new CreateIndexModel<Name>(Builders<Name>.IndexKeys.Text(x => x.PrimaryName));
            await _databaseContext.Names.Indexes.CreateOneAsync(indexModel);

            IAsyncCursor<MongoDB.Bson.BsonDocument> indexes = await _databaseContext.Names.Indexes.ListAsync();

            var partialQuery = queryable.Where(x => x.PrimaryName.Contains("Fonda") ||
                                                    x.PrimaryName.Contains("Coppola"));

            var list = await partialQuery.Take(5)
                                         .Select((x) => new { x.PrimaryName, x.PrimaryProfession })
                                         .ToListAsync();

            int count = await partialQuery.CountAsync();

            IEnumerable<BsonDocument> enumerable = indexes.ToEnumerable();
            Result result = new Result
            {
                Indexes = enumerable,
                IndexesCount = (await _databaseContext.Names.Indexes.ListAsync()).ToEnumerable().Count(),
                People = list,
                Count = count
            };
            return result;
        }

        public sealed class Result
        {
            public object? Indexes { get; set; }
            public int IndexesCount { get; set; }
            public int Count { get; set; }
            public object? People { get; set; }
        }
    }
}
