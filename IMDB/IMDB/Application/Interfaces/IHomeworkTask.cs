namespace IMDB.Application.Interfaces
{
    using System.Threading.Tasks;

    public interface IHomeworkTask
    {
        Task<object> RunAsync();
    }
}
