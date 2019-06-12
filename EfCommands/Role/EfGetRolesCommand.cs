using Application.Commands.Role;
using Application.DataTransfer;
using Application.Responses;
using Application.Searches;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Role
{
    public class EfGetRolesCommand : BaseEfCommand, IGetRolesCommand
    {
        public EfGetRolesCommand(WebNewsContext context) : base(context) { }

        public PagedResponse<RoleDto> Execute(RoleSearch request)
        {
            var query = Context.Roles.AsQueryable();

            if (request.Name != null)
                query = query.Where(r => r.Name.ToLower().Contains(request.Name.ToLower()));

            query = query.Skip((request.PageNumber - 1) * request.PerPage).Take(request.PerPage);

            var totalCount = query.Count();
            var pagesCount = (int)System.Math.Ceiling((double)totalCount / request.PerPage);

            return new PagedResponse<RoleDto>
            {
                CurrentPage = request.PageNumber,
                TotalCount = totalCount,
                PagesCount = pagesCount,
                Data = query.Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Users = r.Users.Select(u => u.FirstName + " " + u.LastName)
                })
            };
        }
    }
}