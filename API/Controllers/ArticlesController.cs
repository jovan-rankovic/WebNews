using API.Helpers;
using Application.Commands.Article;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Helpers;
using Application.Responses;
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

        /// <summary>
        /// Returns and paginates all articles that match the provided query (Logged in)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Articles
        ///     {
        ///        "totalCount": 2,
        ///        "pagesCount": 1,
        ///        "currentPage": 1,
        ///        "data": [
        ///             "title": "Test title",
        ///             "content": "Test content.",
        ///             "image": "img/article/picture.jpg",
        ///             "createdAt": "2019-06-13T20:00:00.00",
        ///             "updatedAt": "2019-06-14T08:30:45.59",
        ///             "authorId": 1,
        ///             "categoryIds": [
        ///                 1,
        ///                 2
        ///             ],
        ///             "hashtagIds": [
        ///                 3,
        ///                 4
        ///             ],
        ///             "author": "Jovan Rankovic",
        ///             "comments": [
        ///                 "Hello.",
        ///                 "World."
        ///             ],
        ///             "categoriesForArticle": [
        ///                 "Test",
        ///                 "Category"
        ///             ],
        ///             "hashtagsForArticle": [
        ///                 "#test",
        ///                 "#hasthag"
        ///             ],
        ///             "id": 1
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <response code="401">If the user is not logged in</response>
        [HttpGet]
        [LoggedIn]
        public ActionResult<PagedResponse<ArticleDto>> Get([FromQuery] ArticleSearch articleSearch)
            => Ok(_searchArticlesCommand.Execute(articleSearch));

        /// <summary>
        /// Returns a specific article (Logged in)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Articles/5
        ///     {
        ///        "title": "Test title",
        ///        "content": "Test content.",
        ///        "image": "img/article/picture.jpg",
        ///        "createdAt": "2019-06-13T20:00:00.00",
        ///        "updatedAt": "2019-06-14T08:30:45.59",
        ///        "authorId": 1,
        ///        "categoryIds": [
        ///             1,
        ///             2
        ///         ],
        ///        "hashtagIds": [
        ///             3,
        ///             4
        ///         ],
        ///        "author": "Jovan Rankovic",
        ///        "comments": [
        ///             "Hello.",
        ///             "World."
        ///         ],
        ///        "categoriesForArticle": [
        ///             "Test",
        ///             "Category"
        ///         ],
        ///        "hashtagsForArticle": [
        ///             "#test",
        ///             "#hasthag"
        ///         ],
        ///        "id": 5
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in</response>
        /// <response code="404">If the article was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpGet("{id}")]
        [LoggedIn]
        public ActionResult<ArticleDto> Get(int id)
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

        /// <summary>
        /// Creates an article (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     POST api/Articles
        ///     {
        ///        "title": "Test title",
        ///        "content": "Test content.",
        ///        "image": "img/article/picture.jpg",
        ///        "authorId" : 1
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="409">If an article with the same title already exists</response>
        /// <response code="500">If another exception happens</response>
        [HttpPost]
        [LoggedIn("Admin")]
        public ActionResult Post([FromForm] ArticleImageUpload articleImageUpload)
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

        /// <summary>
        /// Edits an article (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     PUT api/Articles/5
        ///     {
        ///        "title": "Test title",
        ///        "content": "Test content.",
        ///        "image": "img/article/picture.jpg",
        ///        "authorId" : 2
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the article was not found</response>
        /// <response code="409">If an article with the same title already exists</response>
        /// <response code="500">If another exception happens</response>
        [HttpPut("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Put(int id, [FromForm] ArticleImageUpload articleImageUpload)
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

        /// <summary>
        /// Deletes an article (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Articles/5
        ///     { }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the article was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpDelete("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Delete(int id)
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