namespace IMDB.Application.HomeworkTasks
{
    using System.Threading.Tasks;
    using IMDB.Application.Interfaces;
    using IMDB.Domain.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    [HomeworkExclude]
    public sealed class HomeworkTask05 : IHomeworkTask
    {
        private readonly IDatabaseContext _databaseContext;

        public HomeworkTask05(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<object> RunAsync()
        {
            IMongoQueryable<Title> queryable = _databaseContext.Titles.AsQueryable();

            var list = await queryable.Where(x => x.StartYear == 1942 && (x.OriginalTitle == "Casablanca" || x.PrimaryTitle == "Casablanca"))
                                      .Join(_databaseContext.Crews.AsQueryable(),
                                            title => title.TConst,
                                            crew => crew.TConst,
                                            (title, crew) => new { Title = title, Crew = crew })
                                      .Join(_databaseContext.Names.AsQueryable(),
                                            x => x.Crew.Directors,
                                            names => names.NConst,
                                            (titleCrew, name) => new { TitleCrew = titleCrew, Name = name })
                                      .Select((x) => new { x.Name.PrimaryName, x.Name.BirthYear })
                                      .ToListAsync();

            Result result = new Result
            {
                Data = list,
            };
            return result;
        }

        public sealed class Result
        {
            public object? Data { get; set; }
        }
    }
}
