using Application.Commands.Article;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Searches;
using EfDataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IGetArticlesCommand _searchArticlesCommand;
        private readonly IGetArticleCommand _getOneArticleCommand;
        private readonly ICreateArticleCommand _createArticleCommand;
        private readonly IEditArticleCommand _editArticleCommand;
        private readonly IDeleteArticleCommand _deleteArticleCommand;
        private readonly WebNewsContext _context;

        public ArticlesController(IGetArticlesCommand searchArticlesCommand, IGetArticleCommand getOneArticleCommand, ICreateArticleCommand createArticleCommand, IEditArticleCommand editArticleCommand, IDeleteArticleCommand deleteArticleCommand, WebNewsContext context)
        {
            _searchArticlesCommand = searchArticlesCommand;
            _getOneArticleCommand = getOneArticleCommand;
            _createArticleCommand = createArticleCommand;
            _editArticleCommand = editArticleCommand;
            _deleteArticleCommand = deleteArticleCommand;
            _context = context;
        }

        // GET: Articles
        public ActionResult Index([FromQuery] ArticleSearch articleSearch)
            => View(_searchArticlesCommand.Execute(articleSearch));

        // GET: Articles/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Comments = _context.Comments
                .Where(c => c.ArticleId == id)
                .OrderByDescending(c => c.UpdatedAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Text = c.Text,
                    User = c.User.FirstName + " " + c.User.LastName,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                });

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
            ViewBag.Categories = _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                });

            ViewBag.Hashtags = _context.Hashtags
                .Select(h => new HashtagDto
                {
                    Id = h.Id,
                    Tag = h.Tag
                });

            ViewBag.Users = _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                });

            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleDto articleDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "An error occured.";
                return View();
            }

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
            ViewBag.Categories = _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                });

            ViewBag.Hashtags = _context.Hashtags
                .Select(h => new HashtagDto
                {
                    Id = h.Id,
                    Tag = h.Tag
                });

            ViewBag.Users = _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                });

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
            {
                TempData["error"] = "An error occured.";
                return View();
            }

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