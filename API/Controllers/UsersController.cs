using API.Helpers;
using Application.Commands.User;
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

        // GET: api/Users
        [HttpGet]
        [LoggedIn("Admin")]
        public IActionResult Get([FromQuery] UserSearch userSearch)
            => Ok(_searchUsersCommand.Execute(userSearch));

        // GET: api/Users/5
        [HttpGet("{id}")]
        [LoggedIn("Admin")]
        public IActionResult Get(int id)
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

        // POST: api/Users
        [HttpPost]
        [LoggedIn("Admin")]
        public IActionResult Post([FromBody] UserDto userDto)
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

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [LoggedIn("Admin")]
        public IActionResult Put(int id, [FromBody] UserDto userDto)
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [LoggedIn("Admin")]
        public IActionResult Delete(int id)
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