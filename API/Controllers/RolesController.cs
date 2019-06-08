using Application.Commands.Role;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Searches;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IGetRolesCommand _searchRolesCommand;
        private readonly IGetRoleCommand _getOneRoleCommand;
        private readonly ICreateRoleCommand _createRoleCommand;
        private readonly IEditRoleCommand _editRoleCommand;
        private readonly IDeleteRoleCommand _deleteRoleCommand;

        public RolesController(IGetRolesCommand searchRolesCommand, IGetRoleCommand getOneRoleCommand, ICreateRoleCommand createRoleCommand, IEditRoleCommand editRoleCommand, IDeleteRoleCommand deleteRoleCommand)
        {
            _searchRolesCommand = searchRolesCommand;
            _getOneRoleCommand = getOneRoleCommand;
            _createRoleCommand = createRoleCommand;
            _editRoleCommand = editRoleCommand;
            _deleteRoleCommand = deleteRoleCommand;
        }

        // GET: api/Roles
        [HttpGet]
        public IActionResult Get([FromQuery] RoleSearch roleSearch)
            => Ok(_searchRolesCommand.Execute(roleSearch));

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_getOneRoleCommand.Execute(id));
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST: api/Roles
        [HttpPost]
        public IActionResult Post([FromBody] RoleDto roleDto)
        {
            try
            {
                _createRoleCommand.Execute(roleDto);
                return NoContent();
            }
            catch (EntityAlreadyExistsException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RoleDto roleDto)
        {
            try
            {
                _editRoleCommand.Execute((id, roleDto));
                return NoContent();
            }
            catch (EntityNotFoundException e)
            {
                if (e.Message == "Role not found.")
                    return NotFound(e.Message);

                return UnprocessableEntity(e.Message);
            }
            catch (EntityAlreadyExistsException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _deleteRoleCommand.Execute(id);
                return NoContent();
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}