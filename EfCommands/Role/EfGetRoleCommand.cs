using Application.Commands.Role;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EfCommands.Role
{
    public class EfGetRoleCommand : BaseEfCommand, IGetRoleCommand
    {
        public EfGetRoleCommand(WebNewsContext context) : base(context) { }

        public RoleDto Execute(int request)
        {
            var role = Context.Roles
                .Include(r => r.Users)
                .FirstOrDefault(r => r.Id == request);

            if (role == null)
                throw new EntityNotFoundException("Role");

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Users = role.Users.Select(u => u.FirstName + " " + u.LastName)
            };
        }
    }
}