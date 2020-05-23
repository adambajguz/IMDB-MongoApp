namespace IMDB.Application.HomeworkTasks
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HomeworkExcludeAttribute : Attribute
    {
        public HomeworkExcludeAttribute()
        {

        }
    }
}
