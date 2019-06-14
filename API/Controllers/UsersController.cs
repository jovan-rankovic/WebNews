using API.Helpers;
using Application.Commands.User;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Searches;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IGetUsersCommand _searchUsersCommand;
        private readonly IGetUserCommand _getOneUserCommand;
        private readonly ICreateUserCommand _createUserCommand;
        private readonly IEditUserCommand _editUserCommand;
        private readonly IDeleteUserCommand _deleteUserCommand;

        public UsersController(IGetUsersCommand searchUsersCommand, IGetUserCommand getOneUserCommand, ICreateUserCommand createUserCommand, IEditUserCommand editUserCommand, IDeleteUserCommand deleteUserCommand)
        {
            _searchUsersCommand = searchUsersCommand;
            _getOneUserCommand = getOneUserCommand;
            _createUserCommand = createUserCommand;
            _editUserCommand = editUserCommand;
            _deleteUserCommand = deleteUserCommand;
        }

        /// <summary>
        /// Returns all users that match provided query, and paginates them (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Users
        ///     {
        ///        "totalCount": 2,
        ///        "pagesCount": 1,
        ///        "currentPage": 1,
        ///        "data": [
        ///             "firstName": "Test",
        ///             "lastName": "Dummy",
        ///             "email": "test@gmail.com",
        ///             "roleId": 1,
        ///             "role": "Role",
        ///             "articles": [
        ///                 "Test",
        ///                 "Article"
        ///             ],
        ///             "comments": [
        ///                 "Hello.",
        ///                 "World."
        ///             ],
        ///             "id": 1
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <response code="401">If the user is not logged in as admin</response>
        [HttpGet]
        [LoggedIn("Admin")]
        public ActionResult<IEnumerable<UserDto>> Get([FromQuery] UserSearch userSearch)
            => Ok(_searchUsersCommand.Execute(userSearch));

        /// <summary>
        /// Returns a specific user (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Users/5
        ///     {
        ///        "firstName": "Test",
        ///        "lastName": "Dummy",
        ///        "email": "test@gmail.com",
        ///        "roleId": 1,
        ///        "role": "Role",
        ///        "articles": [
        ///             "Test",
        ///             "Article"
        ///         ],
        ///        "comments": [
        ///             "Hello.",
        ///             "World."
        ///         ],
        ///        "id": 5
        ///     }
        ///     
        /// </remarks>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the user was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpGet("{id}")]
        [LoggedIn("Admin")]
        public ActionResult<UserDto> Get(int id)
        {
            try
            {
                return Ok(_getOneUserCommand.Execute(id));
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
        /// Creates a user (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     POST api/Users
        ///     {
        ///        "firstName": "Test",
        ///        "lastName": "Dummy",
        ///        "email": "test@gmail.com",
        ///        "password": "passw0rd",
        ///        "roleId": 1
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="409">If a user with the same email already exists</response>
        /// <response code="500">If another exception happens</response>
        [HttpPost]
        [LoggedIn("Admin")]
        public ActionResult Post([FromBody] UserDto userDto)
        {
            try
            {
                _createUserCommand.Execute(userDto);
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
        /// Edits a user (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     PUT api/Users/5
        ///     {
        ///        "firstName": "Test",
        ///        "lastName": "Dummy",
        ///        "email": "dummy@gmail.com",
        ///        "password": "passw0rd",
        ///        "roleId": 1
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the user was not found</response>
        /// <response code="409">If a user with the same email already exists</response>
        /// <response code="500">If another exception happens</response>
        [HttpPut("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Put(int id, [FromBody] UserDto userDto)
        {
            try
            {
                _editUserCommand.Execute((id, userDto));
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
        /// Deletes a user (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Users/5
        ///     { }
        ///     
        /// </remarks>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the user was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpDelete("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                _deleteUserCommand.Execute(id);
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