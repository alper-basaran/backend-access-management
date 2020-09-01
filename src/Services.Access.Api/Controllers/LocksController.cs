using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Access.Model;
using Services.Access.Api.Mappers;
using Services.Access.Api.Helpers;
using Services.Access.Domain.Interfaces;
using Services.Access.Domain.Interfaces.Repositories;
using Services.Access.Domain.Model;
using static Services.Access.Domain.Model.Enums;
using Services.Access.Domain.Interfaces.Services;
using Services.Common.Auth.Model;
using Services.Access.Domain.Exceptions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection.Metadata;
using Services.Access.Api.Model;

namespace Services.Access.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class LocksController : ControllerBase
    {

        private readonly ILogger<LocksController> _logger;
        private readonly ILockService _lockService;
        private readonly ILockRepository _lockRepository;
        private readonly IDeviceBusService _deviceBusService;

        private const string ResourceType = "Lock";
        public LocksController(ILogger<LocksController> logger, ILockService lockService
            , ILockRepository lockRepository, IDeviceBusService deviceBusService)
        {
            _lockService = lockService;
            _lockRepository = lockRepository;
            _deviceBusService = deviceBusService;
            _logger = logger;
        }

        /// <summary>
        /// Returns all the locks the current user is authorized to access
        /// Results can be paginated by supplying skip and take parameters in querystring
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] Guid? siteId = null, [FromQuery] Guid? userId = null, [FromQuery] int skip = 0, [FromQuery] int take = 0)
        {
            IEnumerable<Lock> locks = null;
            var user = Request.GetCurrentUser();
            var isAdmin = user.IsAdministrator();
            try
            {
                if (siteId.HasValue && siteId.Value != Guid.Empty)
                {
                    locks = await _lockService.GetLocksOfUserBySiteAsync(user.Id, siteId.Value, isAdmin);
                }
                else
                {
                    locks = await _lockService.GetLocksOfUserAsync(user.Id, isAdmin);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while getting locks of user with exception: {e}");
                return e.Handle();
            }

            if (locks != null && locks.Any())
            {
                var locksDtos = locks.Select(l =>
                {
                    var dto = l.ToDto();
                    dto.State = _deviceBusService.GetLockState(dto.Id);
                    return dto;
                });
                return Ok(locks);
            }
            return NoContent();
        }

        /// <summary>
        /// Returns the lock with given id
        /// </summary>
        /// <param name="id">Identifier of the lock</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = Request.GetCurrentUser();
            var isAdmin = user.IsAdministrator();
            try
            {
                var domainLock = await _lockService.GetLockOfUserByIdAsync(user.Id, id, isAdmin);
                var lockDto = domainLock.ToDto();
                lockDto.State = _deviceBusService.GetLockState(id);

                return Ok(lockDto);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while getting lock with id: {id} with given exception: {e}");
                return e.Handle();
            }
        }

        [HttpPatch("{id}/activate")]
        public IActionResult Activate(Guid id)
        {
            try
            {
                var user = Request.GetCurrentUser();
                var result = _lockService.ActivateLock(user.Id, id);
                return Ok(new ValidResponse { Message = $"Action has been performed on device with id: {id},  Operation result: {Enum.GetName(typeof(LockState), result)}" });
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured activating lock with given exception: {e}");
                return e.Handle();
            }
        }

        /// <summary>
        /// Creates a new lock with given values
        /// </summary>
        /// <param name="value">Paramaters of the new lock</param>
        [HttpPost]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Post([FromBody] LockInsertDto lockDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var domainLock = lockDto.ToDomain();
                    var created = await _lockRepository.InsertAsync(domainLock);
                    var createdDto = created.ToDto();

                    createdDto.State = LockState.Undefined;

                    return CreatedAtAction(nameof(Get), new { id = created.Id }, createdDto);
                }
                catch (Exception e)
                {
                    _logger.LogError($"An error occured adding new lock with given exception: {e}");
                    return e.Handle();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        /// <summary>
        /// Updates a given lock with values specified in the request body
        /// </summary>
        /// <param name="id">Identifier of the site</param>
        /// <param name="value">Updated values of the site</param>
        [HttpPut("{id}")]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Put(Guid id, [FromBody] LockUpdateDto lockDto)
        {
            try
            {
                var domainLock = await _lockRepository.GetByIDAsync(id);
                domainLock.Update(lockDto);
                await _lockRepository.UpdateAsync(domainLock);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while updating lock with id: {id} with given exception: {e}");
                return e.Handle();
            }
        }

        /// <summary>
        /// Deletes the site with given id
        /// </summary>
        /// <param name="id">Identifier of the site</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _lockRepository.DeleteAsync(id);

                return Ok(deleted);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while performing delete operation with given exception: {e}");
                return e.Handle();
            }

        }
    }
}
