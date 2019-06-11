﻿using Application.Commands.Comment;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Searches;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers
{
    public class CommentsController : Controller
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

        // GET: Comments
        public ActionResult Index([FromQuery] CommentSearch commentSearch)
            => View(_searchCommentsCommand.Execute(commentSearch));

        // GET: Comments/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                return View(_getOneCommentCommand.Execute(id));
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

        // GET: Comments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentDto commentDto)
        {
            if (!ModelState.IsValid)
                TempData["error"] = "An error occured.";

            try
            {
                _createCommentCommand.Execute(commentDto);
                TempData["success"] = "Comment posted.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return View();
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_getOneCommentCommand.Execute(id));
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

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CommentDto commentDto)
        {
            if (!ModelState.IsValid)
                TempData["error"] = "An error occured.";

            try
            {
                _editCommentCommand.Execute((id, commentDto));
                TempData["success"] = "Comment modified.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return View();
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(short id)
        {
            try
            {
                return View(_getOneCommentCommand.Execute(id));
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

        // POST: Comments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                _deleteCommentCommand.Execute(id);
                TempData["success"] = "Comment deleted.";
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