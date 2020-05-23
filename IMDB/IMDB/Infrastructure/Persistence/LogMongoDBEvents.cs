namespace IMDB.Infrastructure.Persistence
{
    using System;
    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Driver.Core.Events;
    using Serilog;

    public class LogMongoDBEvents : IEventSubscriber
    {
        private readonly JsonWriterSettings _jsonSettings;
        private readonly ReflectionEventSubscriber _subscriber;

        public LogMongoDBEvents(JsonWriterSettings jsonSettings)
        {
            _jsonSettings = jsonSettings;
            _subscriber = new ReflectionEventSubscriber(this);
        }

        public bool TryGetEventHandler<TEvent>(out Action<TEvent> handler)
        {
            bool handled = _subscriber.TryGetEventHandler(out handler);
            if (!handled)
            {
                handler = e => Log.Debug(e?.ToString());
            }

            return true;
        }

        public void Handle(CommandStartedEvent e)
        {
            Log.Information("Command Started: {Event}, Command {Command}",
                            e.CommandName,
                            e.Command.ToJson(writerSettings: _jsonSettings));
        }

        public void Handle(CommandSucceededEvent e)
        {
            Log.Information("Command Succeeded: {Event}, Duration {Duration}",
                            e.CommandName,
                            e.Duration);
        }
    }
}

