namespace IMDB.Application.HomeworkTasks
{
    using System.Linq;
    using System.Threading.Tasks;
    using IMDB.Application.Interfaces;
    using IMDB.Domain.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    [HomeworkExclude]
    public sealed class HomeworkTask06 : IHomeworkTask
    {
        private readonly IDatabaseContext _databaseContext;

        public HomeworkTask06(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<object> RunAsync()
        {
            IMongoQueryable<Title> queryable = _databaseContext.Titles.AsQueryable();

            var list = await queryable.Where(x => x.StartYear >= 2007 && x.StartYear <= 2009)
                                      .GroupBy(x => x.TitleType)
                                      .Select(n => new
                                      {
                                          TitleType = n.Key,
                                          Count = n.Count()
                                      })
                                      .ToListAsync();

            Result result = new Result
            {
                TitlesStatistics = list,
            };
            return result;
        }

        public sealed class Result
        {
            public object? TitlesStatistics { get; set; }
        }
    }
}
