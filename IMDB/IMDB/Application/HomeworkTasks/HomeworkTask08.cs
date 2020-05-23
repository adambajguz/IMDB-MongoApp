namespace IMDB.Application.HomeworkTasks
{
    using System.Linq;
    using System.Threading.Tasks;
    using IMDB.Application.Interfaces;
    using IMDB.Domain.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    [HomeworkExclude]
    public sealed class HomeworkTask08 : IHomeworkTask
    {
        private readonly IDatabaseContext _databaseContext;

        public HomeworkTask08(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<object> RunAsync()
        {
            IMongoQueryable<Rating> queryableRating = _databaseContext.Ratings.AsQueryable();
            IMongoQueryable<Title> queryableTitle = _databaseContext.Titles.AsQueryable();

            var partialQuery = queryableRating.Where(x => x.AverageRating == 10)
                                              .Join(_databaseContext.Titles.AsQueryable(),
                                                    rating => rating.TConst,
                                                    title => title.TConst,
                                                    (rating, title) => new { Rating = rating, Title = title })
                                              .Select((x) => new { x.Title.Id, x.Rating.AverageRating });

            var listRatings = await queryableRating.Where(x => x.AverageRating == 10)
                                                   .Take(10)
                                                   .Join(_databaseContext.Titles.AsQueryable(),
                                                         rating => rating.TConst,
                                                         title => title.TConst,
                                                         (rating, title) => new { Rating = rating, Title = title })
                                                   .Select((x) => new { x.Title.Id, x.Rating.AverageRating })
                                                   .ToListAsync();

            await partialQuery.ForEachAsync(async (x) =>
            {
                FilterDefinition<Title> filter = Builders<Title>.Filter.Eq(title => title.Id, x.Id);
                UpdateDefinition<Title> update = Builders<Title>.Update.Set("max", 1);

                UpdateResult updateResult = await _databaseContext.Titles.UpdateOneAsync(filter, update);
            });

            var listMax = await queryableTitle.Where(x => x.Max == 1)
                                              .Take(10)
                                              .ToListAsync();

            Result result = new Result
            {
                TitlesWithRatings = listRatings,
                TitlesWithMax = listMax
            };
            return result;
        }

        public sealed class Result
        {
            public object? TitlesWithRatings { get; set; }
            public object? TitlesWithMax { get; set; }
        }
    }
}
