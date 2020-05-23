namespace IMDB.Domain.Entities.Base
{
    using MongoDB.Bson;

    public interface IBaseEntity
    {
        ObjectId Id { get; set; }
    }
}
