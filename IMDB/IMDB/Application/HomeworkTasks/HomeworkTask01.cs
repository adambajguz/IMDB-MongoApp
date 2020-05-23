namespace IMDB.Application.HomeworkTasks
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using IMDB.Application.Interfaces;
    using MongoDB.Driver;

    [HomeworkExclude]
    public sealed class HomeworkTask01 : IHomeworkTask
    {
        private readonly IDatabaseContext _databaseContext;

        public HomeworkTask01(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<object> RunAsync()
        {
            IAsyncCursor<string> collections = await _databaseContext.Db.ListCollectionNamesAsync();

            Result result = new Result
            {
                CollectionNames = collections.ToEnumerable()
            };
            return result;
        }

        public sealed class Result
        {
            public IEnumerable<string>? CollectionNames { get; set; }
        }
    }
}
