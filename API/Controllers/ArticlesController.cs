﻿using Application.Commands.Article;
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
    public class ArticlesController : ControllerBase
    {
        private readonly IGetArticlesCommand _searchArticlesCommand;
        private readonly IGetArticleCommand _getOneArticleCommand;
        private readonly ICreateArticleCommand _createArticleCommand;
        private readonly IEditArticleCommand _editArticleCommand;
        private readonly IDeleteArticleCommand _deleteArticleCommand;

        public ArticlesController(IGetArticlesCommand searchArticlesCommand, IGetArticleCommand getOneArticleCommand, ICreateArticleCommand createArticleCommand, IEditArticleCommand editArticleCommand, IDeleteArticleCommand deleteArticleCommand)
        {
            _searchArticlesCommand = searchArticlesCommand;
            _getOneArticleCommand = getOneArticleCommand;
            _createArticleCommand = createArticleCommand;
            _editArticleCommand = editArticleCommand;
            _deleteArticleCommand = deleteArticleCommand;
        }

        // GET: api/Articles
        [HttpGet]
        public IActionResult Get([FromQuery] ArticleSearch articleSearch)
            => Ok(_searchArticlesCommand.Execute(articleSearch));

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_getOneArticleCommand.Execute(id));
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

        // POST: api/Articles
        [HttpPost]
        public IActionResult Post([FromBody] ArticleDto articleDto)
        {
            try
            {
                _createArticleCommand.Execute(articleDto);
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

        // PUT: api/Articles/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ArticleDto articleDto)
        {
            try
            {
                _editArticleCommand.Execute((id, articleDto));
                return NoContent();
            }
            catch (EntityNotFoundException e)
            {
                if (e.Message == "Article not found.")
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
                _deleteArticleCommand.Execute(id);
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