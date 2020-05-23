namespace IMDB.Application.HomeworkTasks
{
    using System.Linq;
    using System.Threading.Tasks;
    using IMDB.Application.Interfaces;
    using IMDB.Domain.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    //[HomeworkExclude]
    public sealed class HomeworkTask07 : IHomeworkTask
    {
        private readonly IDatabaseContext _databaseContext;

        public HomeworkTask07(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<object> RunAsync()
        {
            IMongoQueryable<Title> queryable = _databaseContext.Titles.AsQueryable();

            int numberOfMatchingElements = await queryable.Where(x => x.StartYear >= 1994 && x.StartYear <= 1996 &&
                                                                      x.Genres.ToLower().Contains("document"))
                                                          .Select(e => e.TConst)
                                                          .Distinct()
                                                          .CountAsync();

            var list = await queryable.Where(x => x.StartYear >= 1994 && x.StartYear <= 1996 &&
                                                  x.Genres.ToLower().Contains("document"))
                                      //.Take(100)
                                      .Select((x) => new { x.TConst, x.PrimaryTitle, x.OriginalTitle, x.StartYear })
                                      .Join(_databaseContext.Ratings.AsQueryable(),
                                            title => title.TConst,
                                            rating => rating.TConst,
                                            (title, rating) => new { title.PrimaryTitle, title.OriginalTitle, title.StartYear, rating.AverageRating })
                                      .OrderByDescending(x => x.AverageRating)
                                      .Take(10)
                                      .ToListAsync();
                                     


            Result result = new Result
            {
                TitlesWithRatings = list,
                NumberOfMatchingElements = numberOfMatchingElements
            };
            return result;
        }

        public sealed class Result
        {
            public object? TitlesWithRatings { get; set; }
            public int NumberOfMatchingElements { get; set; }
        }
    }
}
