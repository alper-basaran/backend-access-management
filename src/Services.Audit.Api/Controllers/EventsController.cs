using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DnsClient.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Services.Audit.Api.Model;
using Services.Audit.Domain.Interfaces;
using Services.Audit.Domain.Model;

namespace Services.Audit.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventsController> _logger;
        public EventsController(IEventRepository eventRepository, IMapper mapper, ILogger<EventsController> logger)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _logger = logger;
        }
        // GET: api/Events
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var evt = await _eventRepository.GetEventAsync(id);
                if (evt != null)
                {
                    var eventDto = _mapper.Map<EventDto>(evt);
                    return Ok(eventDto);
                }
                return NotFound($"Event with id: {id} could not be found");
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while retrieveing event {id} from database: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Events/5
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int skip = 0, [FromQuery] int take = 0)
        {
            if (take < 1) 
                take = 1;
            try
            {
                var events = await _eventRepository.GetEventsAsync(skip, take);
                if (events?.Any() == true)
                {
                    var eventDtos = events.Select(_mapper.Map<EventDto>);
                    return Ok(eventDtos);
                }
                return NotFound($"There is no event records available");
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while retrieveing events with pagination parameters skip: {skip} and take: {take} from database: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Events
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventInsertDto eventInsertDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var evt = _mapper.Map<Event>(eventInsertDto);
                    if (evt != null)
                    {
                        await _eventRepository.AddEventAsync(evt);
                        return Ok();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"An error occured while adding event {eventInsertDto} to database: {e}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            return BadRequest($"An error occured while parsing request with parameters {eventInsertDto}");
        }

    }
}
