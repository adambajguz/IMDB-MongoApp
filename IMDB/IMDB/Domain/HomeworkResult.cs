namespace IMDB.Domain
{
    using System;
    //using System.Text.Json;
    using Newtonsoft.Json;

    public class HomeworkResult
    {
        public string TaskName { get; }
        public object Result { get; }

        public HomeworkResult(string taskName, object result)
        {
            TaskName = taskName;
            Result = result;
        }

        public string GetResultText()
        {
            if (Result is Exception ex)
                return ex.ToString();

            try
            {
                return JsonConvert.SerializeObject(Result, Formatting.Indented);

                //return JsonSerializer.Serialize(Result, new JsonSerializerOptions
                //{
                //    WriteIndented = true,
                //});
            }
            catch (Exception serializerExcpetion)
            {
                return serializerExcpetion.ToString();
            }
        }
    }
}
