using API.Helpers;
using Application.Commands.Role;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Responses;
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

        /// <summary>
        /// Returns and paginates all roles that match the provided query (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Roles
        ///     {
        ///         "totalCount": 2,
        ///         "pagesCount": 1,
        ///         "currentPage": 1,
        ///         "data": [
        ///             "name": "Role",
        ///             "Users": [
        ///                 "Test",
        ///                 "User"
        ///             ],
        ///             "id": 1
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <response code="401">If the user is not logged in as admin</response>
        [HttpGet]
        [LoggedIn("Admin")]
        public ActionResult<PagedResponse<RoleDto>> Get([FromQuery] RoleSearch roleSearch)
            => Ok(_searchRolesCommand.Execute(roleSearch));

        /// <summary>
        /// Returns a specific role (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Roles/5
        ///     {
        ///        "name": "Role",
        ///        "Users": [
        ///             "Test",
        ///             "User"
        ///         ],
        ///        "id": 5
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the role was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpGet("{id}")]
        [LoggedIn("Admin")]
        public ActionResult<RoleDto> Get(int id)
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

        /// <summary>
        /// Creates a role (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     POST api/Roles
        ///     {
        ///        "name": "Role"
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="409">If a role with the same name already exists</response>
        /// <response code="500">If another exception happens</response>
        [HttpPost]
        [LoggedIn("Admin")]
        public ActionResult Post([FromBody] RoleDto roleDto)
        {
            try
            {
                _createRoleCommand.Execute(roleDto);
                return StatusCode(StatusCodes.Status201Created);
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

        /// <summary>
        /// Edits a role (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     PUT api/Roles/5
        ///     {
        ///        "name": "Test"
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the role was not found</response>
        /// <response code="409">If a role with the same name already exists</response>
        /// <response code="500">If another exception happens</response>
        [HttpPut("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Put(int id, [FromBody] RoleDto roleDto)
        {
            try
            {
                _editRoleCommand.Execute((id, roleDto));
                return NoContent();
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
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

        /// <summary>
        /// Deletes a role if it isn't the admin role (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Roles/5
        ///     { }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the role was not found or if it's the admin role</response>
        /// <response code="500">If another exception happens</response>
        [HttpDelete("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Delete(int id)
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