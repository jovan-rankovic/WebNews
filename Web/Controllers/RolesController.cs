using Application.Commands.Role;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Searches;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly IGetRolesCommand _searchRolesCommand;
        private readonly IGetRoleCommand _getOneRoleCommand;
        private readonly ICreateRoleCommand _createRoleCommand;
        private readonly IEditRoleCommand _editRoleCommand;
        private readonly IDeleteRoleCommand _deleteRoleCommand;

        public RolesController(IGetRolesCommand searchRolesCommand, IGetRoleCommand getOneRoleCommand, ICreateRoleCommand createRoleCommand, IEditRoleCommand editRoleCommand, IDeleteRoleCommand deleteRoleCommand)
        {
            _searchRolesCommand = searchRolesCommand;
            _getOneRoleCommand = getOneRoleCommand;
            _createRoleCommand = createRoleCommand;
            _editRoleCommand = editRoleCommand;
            _deleteRoleCommand = deleteRoleCommand;
        }

        // GET: Roles
        public ActionResult Index([FromQuery] RoleSearch roleSearch)
        {
            roleSearch.PageNumber = 0;
            var roles = _searchRolesCommand.Execute(roleSearch);
            return View(roles.Data);
        }

        // GET: Roles/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                return View(_getOneRoleCommand.Execute(id));
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

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "An error occured.";
                return View();
            }

            try
            {
                _createRoleCommand.Execute(roleDto);
                TempData["success"] = "Role created.";
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

        // GET: Roles/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_getOneRoleCommand.Execute(id));
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

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "An error occured.";
                return View();
            }

            try
            {
                _editRoleCommand.Execute((id, roleDto));
                TempData["success"] = "Role modified.";
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

        // GET: Roles/Delete/5
        public ActionResult Delete(short id)
        {
            try
            {
                return View(_getOneRoleCommand.Execute(id));
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

        // POST: Roles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                _deleteRoleCommand.Execute(id);
                TempData["success"] = "Role deleted.";
            }
            catch (EntityNotFoundException e)
            {
                TempData["error"] = e.Message;
            }
            catch (EntityDeleteForbiddenException e)
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