namespace IMDB.Application.HomeworkTasks
{
    using System.Threading.Tasks;
    using IMDB.Application.Interfaces;
    using IMDB.Domain.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    [HomeworkExclude]
    public sealed class HomeworkTask04 : IHomeworkTask
    {
        private readonly IDatabaseContext _databaseContext;

        public HomeworkTask04(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<object> RunAsync()
        {
            IMongoQueryable<Title> queryable = _databaseContext.Titles.AsQueryable();

            var list = await queryable.Where(x => x.StartYear == 1930 &&
                                                  x.Genres != null && x.Genres.ToLower().Contains("comedy"))
                                      .OrderByDescending(x => x.RuntimeMinutes)
                                      .Select((x) => new { x.OriginalTitle, x.RuntimeMinutes, x.Genres })
                                      .ToListAsync();

            Result result = new Result
            {
                Titles = list,
            };
            return result;
        }

        public sealed class Result
        {
            public object? Titles { get; set; }
        }
    }
}
