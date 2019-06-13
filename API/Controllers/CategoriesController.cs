using API.Helpers;
using Application.Commands.Category;
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

        // GET: api/Categories
        [HttpGet]
        [LoggedIn]
        public IActionResult Get([FromQuery] CategorySearch categorySearch)
            => Ok(_searchCategoriesCommand.Execute(categorySearch));

        // GET: api/Categories/5
        [HttpGet("{id}")]
        [LoggedIn]
        public IActionResult Get(int id)
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

        // POST: api/Categories
        [HttpPost]
        [LoggedIn("Admin")]
        public IActionResult Post([FromBody] CategoryDto categoryDto)
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

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        [LoggedIn("Admin")]
        public IActionResult Put(int id, [FromBody] CategoryDto categoryDto)
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

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [LoggedIn("Admin")]
        public IActionResult Delete(int id)
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