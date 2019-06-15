using API.Helpers;
using Application.Commands.Category;
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
    public class CategoriesController : ControllerBase
    {
        private readonly IGetCategoriesCommand _searchCategoriesCommand;
        private readonly IGetCategoryCommand _getOneCategoryCommand;
        private readonly ICreateCategoryCommand _createCategoryCommand;
        private readonly IEditCategoryCommand _editCategoryCommand;
        private readonly IDeleteCategoryCommand _deleteCategoryCommand;

        public CategoriesController(IGetCategoriesCommand searchCategoriesCommand, IGetCategoryCommand getOneCategoryCommand, ICreateCategoryCommand createCategoryCommand, IEditCategoryCommand editCategoryCommand, IDeleteCategoryCommand deleteCategoryCommand)
        {
            _searchCategoriesCommand = searchCategoriesCommand;
            _getOneCategoryCommand = getOneCategoryCommand;
            _createCategoryCommand = createCategoryCommand;
            _editCategoryCommand = editCategoryCommand;
            _deleteCategoryCommand = deleteCategoryCommand;
        }

        /// <summary>
        /// Returns and paginates all categories that match the provided query (Logged in)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Categories
        ///     {
        ///        "totalCount": 2,
        ///        "pagesCount": 1,
        ///        "currentPage": 1,
        ///        "data": [
        ///             "name": "Category",
        ///             "articlesInCategory": [
        ///                 "Test",
        ///                 "Article"
        ///             ],
        ///             "id": 1
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <response code="401">If the user is not logged in</response>
        [HttpGet]
        [LoggedIn]
        public ActionResult<PagedResponse<CategoryDto>> Get([FromQuery] CategorySearch categorySearch)
            => Ok(_searchCategoriesCommand.Execute(categorySearch));

        /// <summary>
        /// Returns a specific category (Logged in)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     GET api/Categories/5
        ///     {
        ///        "name": "Category",
        ///        "articlesInCategory": [
        ///             "Test",
        ///             "Article"
        ///         ],
        ///        "id": 5
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in</response>
        /// <response code="404">If the category was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpGet("{id}")]
        [LoggedIn]
        public ActionResult<CategoryDto> Get(int id)
        {
            try
            {
                return Ok(_getOneCategoryCommand.Execute(id));
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
        /// Creates a category (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     POST api/Categories
        ///     {
        ///        "name": "Category"
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="409">If a category with the same name already exists</response>
        /// <response code="500">If another exception happens</response>
        [HttpPost]
        [LoggedIn("Admin")]
        public ActionResult Post([FromBody] CategoryDto categoryDto)
        {
            try
            {
                _createCategoryCommand.Execute(categoryDto);
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
        /// Edits a category (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     PUT api/Categories/5
        ///     {
        ///        "name": "Test"
        ///     }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the category was not found</response>
        /// <response code="409">If a category with the same name already exists</response>
        /// <response code="500">If another exception happens</response>
        [HttpPut("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Put(int id, [FromBody] CategoryDto categoryDto)
        {
            try
            {
                _editCategoryCommand.Execute((id, categoryDto));
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
        /// Deletes a category (Admin)
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     DELETE api/Categories/5
        ///     { }
        ///     
        /// </remarks>
        /// <response code="400">If validation fails</response>
        /// <response code="401">If the user is not logged in as admin</response>
        /// <response code="404">If the category was not found</response>
        /// <response code="500">If another exception happens</response>
        [HttpDelete("{id}")]
        [LoggedIn("Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                _deleteCategoryCommand.Execute(id);
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