namespace IMDB
{
    using System;
    using System.Threading.Tasks;
    using IMDB.Infrastructure.Homework;
    using IMDB.Infrastructure.Persistence;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--enable-logging")
                SerilogConfiguration.ConfigureSerilog();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Authors:");
            Console.WriteLine("  - Adam Bajguz");
            Console.WriteLine("  - Michał Kierzkowski");
            Console.WriteLine(new string('=', Console.WindowWidth));
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            DatabaseContext databaseContext = new DatabaseContext("mongodb://localhost:27017", "IMDB");

            HomeworkCollection homeworkCollection = new HomeworkCollection(databaseContext);
            homeworkCollection.ResolveAndAddHomeworkTasks();

            await foreach (Domain.HomeworkResult result in homeworkCollection.ExecuteAll())
            {
                string name = $"----[{result.TaskName}]".PadRight(Console.WindowWidth, '-');
                Console.WriteLine(name);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(result.GetResultText());
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine();
                Console.WriteLine(new string('-', Console.WindowWidth));
                Console.WriteLine();
            }
        }
    }
}
