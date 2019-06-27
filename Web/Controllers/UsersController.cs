using Application.Commands.User;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Searches;
using EfDataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IGetUsersCommand _searchUsersCommand;
        private readonly IGetUserCommand _getOneUserCommand;
        private readonly ICreateUserCommand _createUserCommand;
        private readonly IEditUserCommand _editUserCommand;
        private readonly IDeleteUserCommand _deleteUserCommand;
        private readonly WebNewsContext _context;

        public UsersController(IGetUsersCommand searchUsersCommand, IGetUserCommand getOneUserCommand, ICreateUserCommand createUserCommand, IEditUserCommand editUserCommand, IDeleteUserCommand deleteUserCommand, WebNewsContext context)
        {
            _searchUsersCommand = searchUsersCommand;
            _getOneUserCommand = getOneUserCommand;
            _createUserCommand = createUserCommand;
            _editUserCommand = editUserCommand;
            _deleteUserCommand = deleteUserCommand;
            _context = context;
        }

        // GET: Users
        public ActionResult Index([FromQuery] UserSearch userSearch)
        {
            userSearch.PageNumber = 0;
            var users = _searchUsersCommand.Execute(userSearch);
            return View(users.Data);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                return View(_getOneUserCommand.Execute(id));
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

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.Roles = _context.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                });

            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "An error occured.";
                return View();
            }

            try
            {
                _createUserCommand.Execute(userDto);
                TempData["success"] = "User created.";
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

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Roles = _context.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                });

            try
            {
                return View(_getOneUserCommand.Execute(id));
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

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "An error occured.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _editUserCommand.Execute((id, userDto));
                TempData["success"] = "User modified.";
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

        // GET: Users/Delete/5
        public ActionResult Delete(short id)
        {
            try
            {
                return View(_getOneUserCommand.Execute(id));
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

        // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                _deleteUserCommand.Execute(id);
                TempData["success"] = "User deleted.";
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