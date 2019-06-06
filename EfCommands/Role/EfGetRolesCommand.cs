using Application.Commands.Role;
using Application.DataTransfer;
using Application.Searches;
using EfDataAccess;
using System.Collections.Generic;
using System.Linq;

namespace EfCommands.Role
{
    public class EfGetRolesCommand : BaseEfCommand, IGetRolesCommand
    {
        public EfGetRolesCommand(WebNewsContext context) : base(context) { }

        public IEnumerable<RoleDto> Execute(RoleSearch request)
        {
            var query = Context.Roles.AsQueryable();

            if (request.Name != null)
                query = query.Where(r => r.Name.ToLower().Contains(request.Name.ToLower()));

            return query.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Users = r.Users.Select(u => u.FirstName + " " + u.LastName)
            });
        }
    }
}