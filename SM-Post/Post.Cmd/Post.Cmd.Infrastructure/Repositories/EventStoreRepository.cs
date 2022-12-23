using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Config;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;

        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

            _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
        }
        public async Task<List<EventModel>> FindByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken)
        {
            return await _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task SaveAsync(EventModel eventModel)
        {
            await _eventStoreCollection.InsertOneAsync(eventModel).ConfigureAwait(false);
        }
    }
}