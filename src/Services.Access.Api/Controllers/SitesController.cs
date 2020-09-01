using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Access.Model;
using Services.Access.Api.Mappers;
using Services.Access.Api.Helpers;
using Services.Access.Domain.Interfaces.Repositories;
using Services.Access.Domain.Model;
using static Services.Access.Domain.Model.Enums;
using Services.Common.Auth.Model;
using Services.Access.Domain.Interfaces.Services;

namespace Services.Access.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SitesController : ControllerBase
    {
        private readonly ISiteRepository _siteRepository;
        private readonly ISiteService _siteService;
        private readonly IDeviceBusService _deviceBusService;
        private readonly ILogger<SitesController> _logger;

        private const string ResourceType = "Site";
        public SitesController(ILogger<SitesController> logger, ISiteRepository siteRepository
            , ISiteService siteService, IDeviceBusService deviceBusService)
        {
            _siteRepository = siteRepository;
            _siteService = siteService;
            _deviceBusService = deviceBusService;
            _logger = logger;
        }

        /// <summary>
        /// Returns all the sites the current user is authorized to access.
        /// Results can be paginated by supplying skip and take parameters in querystring
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] int skip = 0, [FromQuery] int take = 0)
        {
            IEnumerable<Site> sites;

            var user = Request.GetCurrentUser();
            var isAdmin = user.IsAdministrator();
            try
            {
                sites = await _siteService.GetSitesOfUserAsync(user.Id, isAdmin);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while getting sites of user with exception: {e}");
                return e.Handle();
            }
            if (sites != null && sites.Any())
            {
                var siteDtos = sites.Select(s =>
                {
                    var dto = s.ToDto();
                    dto.State = _deviceBusService.GetSiteState(dto.Id);
                    return dto;
                });
                return Ok(siteDtos);
            }
            return NoContent();
        }

        /// <summary>
        /// Returns the site with given id
        /// </summary>
        /// <param name="id">Identifier of the site</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            Site site;
            var user = Request.GetCurrentUser();
            var isAdmin = user.IsAdministrator();

            try
            {
                site = await _siteService.GetSiteOfUserAsync(user.Id, id, isAdmin);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while getting site with id: {id} with exception: {e}");
                return e.Handle();
            }
            if (site != null)
            {
                var siteDto = site.ToDto();
                siteDto.State = _deviceBusService.GetSiteState(siteDto.Id);

                return Ok(siteDto);
            }
            return ResponseHelpers.NotFoundResponse("Site", id);
        }

        /// <summary>
        /// Creates a new site with given values
        /// </summary>
        /// <param name="value">Values of the new site</param>
        [HttpPost]
        [Authorize(Roles = RoleNames.Administrator)]
        public IActionResult Post([FromBody] SiteUpsertDto siteDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var site = siteDto.ToDomain();
                    var createdDto = _siteRepository.Insert(site).ToDto();

                    createdDto.State = SiteState.Undefined;

                    return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                //TODO: Log the exception
                _logger.LogError($"An error occured while inserting new site with given exception: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a given site with values specified in the request body
        /// </summary>
        /// <param name="id">Identifier of the site</param>
        /// <param name="value">Updated values of the site</param>
        [HttpPut("{id}")]
        [Authorize(Roles = RoleNames.Administrator)]
        public IActionResult Put(Guid id, [FromBody] SiteUpsertDto siteDto)
        {
            var site = _siteRepository.GetByID(id);
            if (site == null)
                return ResponseHelpers.NotFoundResponse(ResourceType, id);
            try
            {
                site.Update(siteDto);
                _siteRepository.Update(site);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while updating site with id: {id} with given exception: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes the site with given id
        /// </summary>
        /// <param name="id">Identifier of the site</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.Administrator)]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var deleted = _siteRepository.Delete(id);
                if (deleted == null)
                    return BadRequest($"The site with given {id} could not be found");

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while performing delete operation with given exception: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
