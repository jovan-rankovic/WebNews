using Application.Commands.Article;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Searches;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers
{
    public class ArticlesController : Controller
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

        // GET: Articles
        public ActionResult Index([FromQuery] ArticleSearch articleSearch)
            => View(_searchArticlesCommand.Execute(articleSearch));

        // GET: Articles/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                return View(_getOneArticleCommand.Execute(id));
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

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleDto articleDto)
        {
            if (!ModelState.IsValid)
                TempData["error"] = "An error occured.";

            try
            {
                _createArticleCommand.Execute(articleDto);
                TempData["success"] = "Article posted.";
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

        // GET: Articles/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_getOneArticleCommand.Execute(id));
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

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ArticleDto articleDto)
        {
            if (!ModelState.IsValid)
                TempData["error"] = "An error occured.";

            try
            {
                _editArticleCommand.Execute((id, articleDto));
                TempData["success"] = "Article modified.";
                return RedirectToAction(nameof(Index));
            }
            catch (EntityNotFoundException e)
            {
                TempData["error"] = e.Message;
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

        // GET: Articles/Delete/5
        public ActionResult Delete(short id)
        {
            try
            {
                return View(_getOneArticleCommand.Execute(id));
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

        // POST: Articles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                _deleteArticleCommand.Execute(id);
                TempData["success"] = "Article deleted.";
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