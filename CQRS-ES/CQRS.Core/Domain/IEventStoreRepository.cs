using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace CQRS.Core.Domain
{
    public interface IEventStoreRepository
    {
        Task SaveAsync(EventModel @eventModel);
        
        Task<List<EventModel>> FindByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken);
    }
}