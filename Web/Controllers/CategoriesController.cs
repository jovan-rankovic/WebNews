using Application.Commands.Category;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Searches;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers
{
    public class CategoriesController : Controller
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

        // GET: Categories
        public ActionResult Index([FromQuery] CategorySearch categorySearch)
        {
            categorySearch.PageNumber = 0;
            var categories = _searchCategoriesCommand.Execute(categorySearch);
            return View(categories.Data);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                return View(_getOneCategoryCommand.Execute(id));
            }
            catch (EntityNotFoundException e)
            {
                TempData["error"] = e.Message;
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "An error occured.";
                return View();
            }

            try
            {
                _createCategoryCommand.Execute(categoryDto);
                TempData["success"] = "Category created.";
                return RedirectToAction(nameof(Index));
            }
            catch (EntityAlreadyExistsException e)
            {
                TempData["error"] = e.Message;
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return View();
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_getOneCategoryCommand.Execute(id));
            }
            catch (EntityNotFoundException e)
            {
                TempData["error"] = e.Message;
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "An error occured.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _editCategoryCommand.Execute((id, categoryDto));
                TempData["success"] = "Category modified.";
                return RedirectToAction(nameof(Index));
            }
            catch (EntityNotFoundException e)
            {
                TempData["error"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (EntityAlreadyExistsException e)
            {
                TempData["error"] = e.Message;
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return View();
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(short id)
        {
            try
            {
                return View(_getOneCategoryCommand.Execute(id));
            }
            catch (EntityNotFoundException e)
            {
                TempData["error"] = e.Message;
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                _deleteCategoryCommand.Execute(id);
                TempData["success"] = "Category deleted.";
            }
            catch (EntityNotFoundException e)
            {
                TempData["error"] = e.Message;
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}