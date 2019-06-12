using Application.Commands.User;
using Application.DataTransfer;
using Application.Responses;
using Application.Searches;
using EfDataAccess;
using System.Linq;

namespace EfCommands.User
{
    public class EfGetUsersCommand : BaseEfCommand, IGetUsersCommand
    {
        public EfGetUsersCommand(WebNewsContext context) : base(context) { }

        public PagedResponse<UserDto> Execute(UserSearch request)
        {
            var query = Context.Users.AsQueryable();

            if (request.FirstName != null)
                query = query.Where(u => u.FirstName.ToLower().Contains(request.FirstName.ToLower()));

            if (request.LastName != null)
                query = query.Where(u => u.LastName.ToLower().Contains(request.LastName.ToLower()));

            if (request.Email != null)
                query = query.Where(u => u.Email.ToLower().Contains(request.Email.ToLower()));

            if (request.IsAdmin == true)
                query = query.Where(u => u.Role.Name.ToLower().Contains("admin"));

            query = query.Skip((request.PageNumber - 1) * request.PerPage).Take(request.PerPage);

            var totalCount = query.Count();
            var pagesCount = (int)System.Math.Ceiling((double)totalCount / request.PerPage);

            return new PagedResponse<UserDto>
            {
                CurrentPage = request.PageNumber,
                TotalCount = totalCount,
                PagesCount = pagesCount,
                Data = query.Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.LastName,
                    RoleId = u.RoleId,
                    Role = u.Role.Name,
                    Articles = u.Articles.Select(a => a.Title),
                    Comments = u.Comments.Select(c => c.Text)
                })
            };
        }
    }
}