using API.Helpers;
using Application.Commands.Hashtag;
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

        /// <summary>
        /// Returns all hashtags that match provided query (Logged in)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Hashtags
        ///     {
        ///        "tag": "#hashtag",
        ///        "articlesWithHashtag": [
        ///             "Test",
        ///             "Article"
        ///         ],
        ///        "id": 1
        ///     }
        ///     
        /// </remarks>
        /// <response code="401">If the user is not logged in</response>
        [HttpGet]
        [LoggedIn]
        public ActionResult<IEnumerable<HashtagDto>> Get([FromQuery] HashtagSearch hashtagSearch)
            => Ok(_searchHashtagsCommand.Execute(hashtagSearch));

        /// <summary>
        /// Returns a specific hashtag (Logged in)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Hashtags/5
        ///     {
        ///        "tag": "#hashtag",
        ///        "articlesWithHashtag": [
        ///             "Test",
        ///             "Article"
        ///         ],
        ///        "id": 5
        ///     }
        ///     
        /// </remarks>
        /// <response code="401">If the user is not logged in</response>
        /// <response code="404">If the hashtag was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpGet("{id}")]
        [LoggedIn]
        public ActionResult<HashtagDto> Get(int id)
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

        /// <summary>
        /// Creates a hashtag (Logged in)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     POST api/Hashtags
        ///     {
        ///        "tag": "#hashtag"
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in</response>
        /// <response code="409">If a hashtag with the same tag already exists</response>
        /// <response code="500">If another exception happens</response>
        [HttpPost]
        [LoggedIn]
        public ActionResult Post([FromBody] HashtagDto hashtagDto)
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

        /// <summary>
        /// Edits a hashtag (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     PUT api/Hashtags/5
        ///     {
        ///        "tag": "#test"
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the hashtag was not found</response>
        /// <response code="409">If a hashtag with the same tag already exists</response>
        /// <response code="500">If another exception happens</response>
        [HttpPut("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Put(int id, [FromBody] HashtagDto hashtagDto)
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

        /// <summary>
        /// Deletes a hashtag (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Hashtags/5
        ///     { }
        ///     
        /// </remarks>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the hashtag was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpDelete("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Delete(int id)
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