namespace IMDB.Application.HomeworkTasks
{
    using System.Threading.Tasks;
    using IMDB.Application.Interfaces;
    using IMDB.Domain.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    [HomeworkExclude]
    public sealed class HomeworkTask03 : IHomeworkTask
    {
        private readonly IDatabaseContext _databaseContext;

        public HomeworkTask03(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<object> RunAsync()
        {
            IMongoQueryable<Title> queryable = _databaseContext.Titles.AsQueryable();

            IOrderedMongoQueryable<Title> partialQuery = queryable.Where(x => x.StartYear == 2005 &&
                                                                              x.Genres != null && x.Genres.ToLower().Contains("romance") &&
                                                                              x.RuntimeMinutes > 100 && x.RuntimeMinutes < 120)
                                                                  .OrderBy(x => x.OriginalTitle);

            var list = await partialQuery.Take(5)
                                         .Select((x) => new { x.OriginalTitle, x.PrimaryTitle, x.StartYear, x.Genres, x.RuntimeMinutes })
                                         .ToListAsync();

            int numberOfMatchingElements = await partialQuery.CountAsync();

            Result result = new Result
            {
                Titles = list,
                NumberOfMatchingElements = numberOfMatchingElements
            };
            return result;
        }

        public sealed class Result
        {
            public object? Titles { get; set; }
            public int NumberOfMatchingElements { get; set; }
        }
    }
}

