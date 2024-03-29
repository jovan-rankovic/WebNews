﻿using Application.Commands.Role;
using Application.Exceptions;
using EfDataAccess;

namespace EfCommands.Role
{
    public class EfDeleteRoleCommand : BaseEfCommand, IDeleteRoleCommand
    {
        public EfDeleteRoleCommand(WebNewsContext context) : base(context) { }

        public void Execute(int request)
        {
            var role = Context.Roles.Find(request);

            if (role == null)
                throw new EntityNotFoundException("Role");

            if (role.Name.ToLower().Contains("admin"))
                throw new EntityDeleteForbiddenException("role");

            Context.Roles.Remove(role);

            Context.SaveChanges();
        }
    }
}