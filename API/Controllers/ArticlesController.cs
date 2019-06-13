using API.Helpers;
using Application.Commands.Article;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Helpers;
using Application.Searches;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

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
        [LoggedIn]
        public IActionResult Get([FromQuery] ArticleSearch articleSearch)
            => Ok(_searchArticlesCommand.Execute(articleSearch));

        // GET: api/Articles/5
        [HttpGet("{id}")]
        [LoggedIn]
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
        [LoggedIn("Admin")]
        public IActionResult Post([FromForm] ArticleImageUpload articleImageUpload)
        {
            if (articleImageUpload.ImageFile != null)
            {
                var ext = Path.GetExtension(articleImageUpload.ImageFile.FileName);

                if (!FileUpload.AllowedExtensions.Contains(ext))
                    return UnprocessableEntity("Image extension is not allowed.");
            }

            try
            {
                var articleDto = new ArticleDto
                {
                    Title = articleImageUpload.Title,
                    Content = articleImageUpload.Content,
                    AuthorId = articleImageUpload.AuthorId
                };

                if (articleImageUpload.ImageFile != null)
                {
                    var newFileName = Guid.NewGuid().ToString() + "_" + articleImageUpload.ImageFile.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "article", newFileName);
                    articleImageUpload.ImageFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    articleDto.Image = "img/article/" + newFileName;
                }

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
        [LoggedIn("Admin")]
        public IActionResult Put(int id, [FromForm] ArticleImageUpload articleImageUpload)
        {
            if (articleImageUpload.ImageFile != null)
            {
                var ext = Path.GetExtension(articleImageUpload.ImageFile.FileName);

                if (!FileUpload.AllowedExtensions.Contains(ext))
                    return UnprocessableEntity("Image extension is not allowed.");
            }

            try
            {
                var articleDto = new ArticleDto
                {
                    Title = articleImageUpload.Title,
                    Content = articleImageUpload.Content,
                    AuthorId = articleImageUpload.AuthorId
                };

                if (articleImageUpload.ImageFile != null)
                {
                    var newFileName = Guid.NewGuid().ToString() + "_" + articleImageUpload.ImageFile.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "article", newFileName);
                    articleImageUpload.ImageFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    articleDto.Image = "img/article/" + newFileName;
                }

                _editArticleCommand.Execute((id, articleDto));
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

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        [LoggedIn("Admin")]
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

        public class ArticleImageUpload
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public int AuthorId { get; set; }
            public IFormFile ImageFile { get; set; }
        }
    }
}