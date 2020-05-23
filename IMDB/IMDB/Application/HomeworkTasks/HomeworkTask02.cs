namespace IMDB.Application.HomeworkTasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using IMDB.Application.Interfaces;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    [HomeworkExclude]
    public sealed class HomeworkTask02 : IHomeworkTask
    {
        private readonly IDatabaseContext _databaseContext;

        public HomeworkTask02(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<object> RunAsync()
        {
            IAsyncCursor<string> collections = await _databaseContext.Db.ListCollectionNamesAsync();

            List<CollectionInfo> collectionSizes = new List<CollectionInfo>();
            await collections.ForEachAsync(async (x) =>
            {
                IMongoCollection<object> collection = _databaseContext.Db.GetCollection<object>(x);
                IMongoQueryable<object> mongoQueryable = collection.AsQueryable();

                int size = await mongoQueryable.CountAsync();
                object firstObject = await mongoQueryable.FirstOrDefaultAsync();

                collectionSizes.Add((x, size, firstObject));
            });

            Result result = new Result
            {
                CollectionSizes = collectionSizes
            };
            return result;
        }

        public sealed class Result
        {
            public IEnumerable<CollectionInfo>? CollectionSizes { get; set; }
        }

        public struct CollectionInfo
        {
            public string CollectionName { get; }
            public int Size { get; }
            public object Entity { get; }

            public CollectionInfo(string collectionName, int size, object entity)
            {
                CollectionName = collectionName;
                Size = size;
                Entity = entity;
            }

            public override bool Equals(object? obj)
            {
                return obj is CollectionInfo other &&
                       CollectionName == other.CollectionName &&
                       Size == other.Size && Entity == other.Entity;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(CollectionName, Size, Entity);
            }

            public void Deconstruct(out string collectionName, out int size, out object entity)
            {
                collectionName = CollectionName;
                size = Size;
                entity = Entity;
            }

            public static implicit operator (string CollectionName, int Size, object Entity)(CollectionInfo value)
            {
                return (value.CollectionName, value.Size, value.Entity);
            }

            public static implicit operator CollectionInfo((string CollectionName, int Size, object Entity) value)
            {
                return new CollectionInfo(value.CollectionName, value.Size, value.Entity);
            }
        }
    }
}
