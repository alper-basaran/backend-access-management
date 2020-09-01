using Services.Audit.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Audit.Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<Event> GetEventAsync(Guid id);
        Task<IEnumerable<Event>> GetEventsAsync(int skip, int take);
        Task AddEventAsync(Event @event);
        Task DeleteEventAsync(Guid id);
    }
}
