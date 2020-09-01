using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Services.Audit.Domain.Interfaces;
using Services.Audit.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Audit.Data.Repositories
{
    public class EventRepository : IEventRepository
    {
        private const string CollectionKey = "Events";
    
        private readonly IMongoDatabase _database;
        public EventRepository(IMongoDatabase database)
        {
            _database = database;
        }
        public async Task<Event> GetEventAsync(Guid id)
        {
            var collection = _database.GetCollection<Event>(CollectionKey);
            var items = collection.AsQueryable().ToList();
            return await collection.AsQueryable().FirstOrDefaultAsync(e => e.EventId == id);
        }
        public async Task<IEnumerable<Event>> GetEventsAsync(int skip, int take)
        {
            var collection = _database.GetCollection<Event>(CollectionKey);
            return await collection.AsQueryable().Skip(skip).Take(take).ToListAsync();
        }
        public async Task AddEventAsync(Event @event)
        {
            var collection = _database.GetCollection<Event>(CollectionKey);
            await collection.InsertOneAsync(@event);
        }
        public async Task DeleteEventAsync(Guid id)
        {
            var collection = _database.GetCollection<Event>(CollectionKey);
            await collection.DeleteOneAsync(x => x.EventId == id);
        }
    }
}
