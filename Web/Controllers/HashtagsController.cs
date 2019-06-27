using Application.Commands.Hashtag;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Searches;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers
{
    public class HashtagsController : Controller
    {
        private readonly IGetHashtagsCommand _searchHashtagsCommand;
        private readonly IGetHashtagCommand _getOneHashtagCommand;
        private readonly ICreateHashtagCommand _createHashtagCommand;
        private readonly IEditHashtagCommand _editHashtagCommand;
        private readonly IDeleteHashtagCommand _deleteHashtagCommand;

        public HashtagsController(IGetHashtagsCommand searchHashtagsCommand, IGetHashtagCommand getOneHashtagCommand, ICreateHashtagCommand createHashtagCommand, IEditHashtagCommand editHashtagCommand, IDeleteHashtagCommand deleteHashtagCommand)
        {
            _searchHashtagsCommand = searchHashtagsCommand;
            _getOneHashtagCommand = getOneHashtagCommand;
            _createHashtagCommand = createHashtagCommand;
            _editHashtagCommand = editHashtagCommand;
            _deleteHashtagCommand = deleteHashtagCommand;
        }

        // GET: Hashtags
        public ActionResult Index([FromQuery] HashtagSearch hashtagSearch)
        {
            hashtagSearch.PageNumber = 0;
            var hashtags = _searchHashtagsCommand.Execute(hashtagSearch);
            return View(hashtags.Data);
        }

        // GET: Hashtags/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                return View(_getOneHashtagCommand.Execute(id));
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

        // GET: Hashtags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hashtags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HashtagDto hashtagDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "An error occured.";
                return View();
            }

            try
            {
                _createHashtagCommand.Execute(hashtagDto);
                TempData["success"] = "Hashtag created.";
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

        // GET: Hashtags/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_getOneHashtagCommand.Execute(id));
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

        // POST: Hashtags/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HashtagDto hashtagDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "An error occured.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _editHashtagCommand.Execute((id, hashtagDto));
                TempData["success"] = "Hashtag modified.";
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

        // GET: Hashtags/Delete/5
        public ActionResult Delete(short id)
        {
            try
            {
                return View(_getOneHashtagCommand.Execute(id));
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

        // POST: Hashtags/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                _deleteHashtagCommand.Execute(id);
                TempData["success"] = "Hashtag deleted.";
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