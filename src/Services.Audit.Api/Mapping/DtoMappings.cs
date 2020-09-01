using AutoMapper;
using Services.Audit.Api.Model;
using Services.Audit.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Audit.Api.Mapping
{
    public class DtoMappings : Profile
    {
        public DtoMappings()
        {
            CreateMap<Event, EventDto>();
            CreateMap<EventInsertDto, Event>()
            .ForMember(e => e.EventId, opt => opt.MapFrom<IdResolver>())
            .ForMember(e => e.Created, opt => opt.MapFrom<DateResolver>());
        }

        public class IdResolver : IValueResolver<EventInsertDto, Event, Guid>
        {
            public Guid Resolve(EventInsertDto source, Event destination, Guid destMember, ResolutionContext context)
            {
                return Guid.NewGuid();
            }
        }

        public class DateResolver : IValueResolver<EventInsertDto, Event, DateTime>
        {
            public DateTime Resolve(EventInsertDto source, Event destination, DateTime destMember, ResolutionContext context)
            {
                return DateTime.UtcNow;
            }
        }
    }
}
