using API.Helpers;
using Application.Commands.Comment;
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
    public class CommentsController : ControllerBase
    {
        private readonly IGetCommentsCommand _searchCommentsCommand;
        private readonly IGetCommentCommand _getOneCommentCommand;
        private readonly ICreateCommentCommand _createCommentCommand;
        private readonly IEditCommentCommand _editCommentCommand;
        private readonly IDeleteCommentCommand _deleteCommentCommand;

        public CommentsController(IGetCommentsCommand searchCommentsCommand, IGetCommentCommand getOneCommentCommand, ICreateCommentCommand createCommentCommand, IEditCommentCommand editCommentCommand, IDeleteCommentCommand deleteCommentCommand)
        {
            _searchCommentsCommand = searchCommentsCommand;
            _getOneCommentCommand = getOneCommentCommand;
            _createCommentCommand = createCommentCommand;
            _editCommentCommand = editCommentCommand;
            _deleteCommentCommand = deleteCommentCommand;
        }

        /// <summary>
        /// Returns and paginates all comments that match the provided query (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Comments
        ///     {
        ///        "totalCount": 2,
        ///        "pagesCount": 1,
        ///        "currentPage": 1,
        ///        "data": [
        ///             "text": "Hello World.",
        ///             "createdAt": "2019-06-13T20:00:00.00",
        ///             "updatedAt": "2019-06-14T08:30:45.59",
        ///             "articleId": 1,
        ///             "userId": 1,
        ///             "article": "Test article",
        ///             "user": "Jovan Rankovic",
        ///             "id": 1
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <response code="401">If the user is not logged in as admin</response>
        [HttpGet]
        [LoggedIn("Admin")]
        public ActionResult<PagedResponse<CommentDto>> Get([FromQuery] CommentSearch commentsSearch)
            => Ok(_searchCommentsCommand.Execute(commentsSearch));

        /// <summary>
        /// Returns a specific comment (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Comments/5
        ///     {
        ///        "text": "Hello World.",
        ///        "createdAt": "2019-06-13T20:00:00.00",
        ///        "updatedAt": "2019-06-14T08:30:45.59",
        ///        "articleId": 1,
        ///        "userId": 1,
        ///        "article": "Test article",
        ///        "user": "Jovan Rankovic",
        ///        "id": 5
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the comment was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpGet("{id}")]
        [LoggedIn("Admin")]
        public ActionResult<CommentDto> Get(int id)
        {
            try
            {
                return Ok(_getOneCommentCommand.Execute(id));
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
        /// Creates a comment (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     POST api/Comments
        ///     {
        ///        "text": "Hello World.",
        ///        "articleId": 1,
        ///        "userId": 1,
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="500">If another exception happens</response>
        [HttpPost]
        [LoggedIn("Admin")]
        public ActionResult Post([FromBody] CommentDto commentDto)
        {
            try
            {
                _createCommentCommand.Execute(commentDto);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Edits a comment (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     PUT api/Comments/5
        ///     {
        ///        "text": "Hello World.",
        ///        "articleId": 5,
        ///        "userId": 1,
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the comment was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpPut("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Put(int id, [FromBody] CommentDto commentDto)
        {
            try
            {
                _editCommentCommand.Execute((id, commentDto));
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

        /// <summary>
        /// Deletes a comment (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Comments/5
        ///     { }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the comment was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpDelete("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                _deleteCommentCommand.Execute(id);
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