using API.Helpers;
using Application.Commands.Comment;
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

        // GET: api/Comments
        [HttpGet]
        [LoggedIn]
        public IActionResult Get([FromQuery] CommentSearch commentsSearch)
            => Ok(_searchCommentsCommand.Execute(commentsSearch));

        // GET: api/Comments/5
        [HttpGet("{id}")]
        [LoggedIn]
        public IActionResult Get(int id)
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

        // POST: api/Comments
        [HttpPost]
        [LoggedIn]
        public IActionResult Post([FromBody] CommentDto commentDto)
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

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        [LoggedIn("Admin")]
        public IActionResult Put(int id, [FromBody] CommentDto commentDto)
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [LoggedIn("Admin")]
        public IActionResult Delete(int id)
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