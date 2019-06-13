using API.Helpers;
using Application.Commands.Hashtag;
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
    public class HashtagsController : ControllerBase
    {
        private readonly IGetHashtagsCommand _searchHashtagsCommand;
        private readonly IGetHashtagCommand _getOneHashtagCommand;
        private readonly ICreateHashtagCommand _createHashtagCommand;
        private readonly IEditHashtagCommand _editHashtagCommand;
        private readonly IDeleteHashtagCommand _deleteHashtagCommand;

        public HashtagsController(IGetHashtagsCommand searchHashtagsCommand, IGetHashtagCommand getOneHashtagCommand, ICreateHashtagCommand createHashtagCommand, IEditHashtagCommand editHashtagCommand, IDeleteHashtagCommand deleteHashtagCommand)
        {
            _searchHashtagsCommand = searchHashtagsCommand;
            _getOneHashtagCommand = getOneHashtagCommand;
            _createHashtagCommand = createHashtagCommand;
            _editHashtagCommand = editHashtagCommand;
            _deleteHashtagCommand = deleteHashtagCommand;
        }

        // GET: api/Hashtags
        [HttpGet]
        [LoggedIn]
        public IActionResult Get([FromQuery] HashtagSearch hashtagSearch)
            => Ok(_searchHashtagsCommand.Execute(hashtagSearch));

        // GET: api/Hashtags/5
        [HttpGet("{id}")]
        [LoggedIn]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_getOneHashtagCommand.Execute(id));
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

        // POST: api/Hashtags
        [HttpPost]
        [LoggedIn]
        public IActionResult Post([FromBody] HashtagDto hashtagDto)
        {
            try
            {
                _createHashtagCommand.Execute(hashtagDto);
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

        // PUT: api/Hashtags/5
        [HttpPut("{id}")]
        [LoggedIn("Admin")]
        public IActionResult Put(int id, [FromBody] HashtagDto hashtagDto)
        {
            try
            {
                _editHashtagCommand.Execute((id, hashtagDto));
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
                _deleteHashtagCommand.Execute(id);
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