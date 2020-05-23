namespace IMDB.Infrastructure.Homework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using IMDB.Application.HomeworkTasks;
    using IMDB.Application.Interfaces;
    using IMDB.Common.Extensions;
    using IMDB.Domain;
    using IMDB.Infrastructure.Persistence;

    public class HomeworkCollection
    {
        private readonly List<IHomeworkTask> _homeworkTasks = new List<IHomeworkTask>();
        private readonly DatabaseContext _databaseContext;

        public HomeworkCollection(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IHomeworkTask AddHomeworkTask<T>()
            where T : IHomeworkTask
        {
            IHomeworkTask task = Activator.CreateInstance(typeof(T), _databaseContext) as IHomeworkTask ?? throw new NullReferenceException(typeof(T).FullName);
            _homeworkTasks.Add(task);

            return task;
        }

        public IHomeworkTask AddHomeworkTask(Type t)
        {
            IHomeworkTask task = Activator.CreateInstance(t, _databaseContext) as IHomeworkTask ?? throw new NullReferenceException(t.FullName);
            _homeworkTasks.Add(task);

            return task;
        }

        public void ResolveAndAddHomeworkTasks()
        {
            Assembly currentAssemlby = Assembly.GetExecutingAssembly();

            IEnumerable<Type> types = currentAssemlby.GetTypes()
                                                     .Where(x => x.GetCustomAttribute<HomeworkExcludeAttribute>() is null &&
                                                                 x.GetInterfaces()
                                                                  .Contains(typeof(IHomeworkTask)));
            if (types.Any())
                types.ForEach((x) => AddHomeworkTask(x));
        }

        public IEnumerable<IHomeworkTask> GetHomeworkTasks()
        {
            return _homeworkTasks.AsEnumerable();
        }

        public async IAsyncEnumerable<HomeworkResult> ExecuteAll()
        {
            foreach (IHomeworkTask task in _homeworkTasks)
            {
                HomeworkResult homeworkResult;
                try
                {
                    object result = await task.RunAsync();
                    homeworkResult = new HomeworkResult(task.GetType().Name, result);
                }
                catch (Exception ex)
                {
                    homeworkResult = new HomeworkResult(task.GetType().Name, ex);
                }

                yield return homeworkResult;
            }

            yield break;
        }
    }
}
