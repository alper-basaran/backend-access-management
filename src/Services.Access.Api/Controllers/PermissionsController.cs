using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Access.Domain.Interfaces.Repositories;
using Services.Access.Domain.Model;
using Services.Access.Api.Mappers;
using Services.Access.Api.Helpers;
using Microsoft.Extensions.Logging;
using Services.Access.Model;
using Services.Common.Auth.Model;

namespace Services.Access.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly ILogger<PermissionsController> _logger;
        private readonly IPermissionRepository _permissionRepository;

        private const string ResourceType  = "Permission";

        public PermissionsController(ILogger<PermissionsController> logger, IPermissionRepository permissionRepository)
        {
            _logger = logger;
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// Returns a list of permissions that the current user has permission to see
        /// </summary>
        /// <param name="lock">Filters permissions by associated lock object</param>
        /// <param name="site">Filters permissions by associated site object</param>
        /// <param name="skip">Number of results to skip for pagination </param>
        /// <param name="take">Number of results to retrieve</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = RoleNames.Administrator)]
        public IActionResult Get([FromQuery] Guid? @lock, [FromQuery] Guid? site, [FromQuery] int skip = 0, [FromQuery] int take = 0)
        {
            IEnumerable<Permission> permissions;

            try
            {
                if (@lock.HasValue && @lock.Value != Guid.Empty)
                    permissions = _permissionRepository.GetPermissionsByLock(@lock.Value);

                else if (site.HasValue && site.Value != Guid.Empty)
                    permissions = _permissionRepository.GetPermissionsBySite(site.Value);

                else
                    permissions = _permissionRepository.GetAll();

                if (permissions.Any())
                {
                    var permissionDto = permissions.Select(p => p.ToDto());
                    return Ok(permissionDto);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while retrieving permissions with following parameters:" + 
                    $" lock = {@lock} site = {site}, with exception: {e}");
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns the permission with given ID
        /// </summary>
        /// <param name="id">Identifier of the permission</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = RoleNames.Administrator)]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var permission = _permissionRepository.GetByID(id);
                if (permission != null)
                {
                    return Ok(permission.ToDto());
                }
                else
                    return ResponseHelpers.NotFoundResponse(ResourceType, id);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while retrieving permission with given Id: {id} with exception: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
           
        }

        /// <summary>
        /// Inserts a new permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = RoleNames.Administrator)]
        public IActionResult Post([FromBody] PermissionInsertDto permission)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var domainPermission = permission.ToDomain();
                    var created = _permissionRepository.Insert(domainPermission);
                    var createdDto = created.ToDto();

                    return CreatedAtAction(nameof(Get), new { id = created.Id }, createdDto);
                }
                catch (Exception e)
                {
                    _logger.LogError($"An error occured while creating permission with given parameters" + 
                        $" userId: {permission.UserId}, objectId = {permission.ObjectId} with exception: {e}");
                    
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
                return BadRequest(ModelState);
        }

        /// <summary>
        /// Removes the specified permission
        /// </summary>
        /// <param name="id">Identifier of the permission object</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.Administrator)]
        public IActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id needs to be a non-empty valid identifier");
            try
            {
                var deleted = _permissionRepository.Delete(id);
                if (deleted == null)
                    return ResponseHelpers.NotFoundResponse(ResourceType, id);

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
