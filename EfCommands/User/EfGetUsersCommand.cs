using Application.Commands.User;
using Application.DataTransfer;
using Application.Searches;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EfCommands.User
{
    public class EfGetUsersCommand : BaseEfCommand, IGetUsersCommand
    {
        public EfGetUsersCommand(WebNewsContext context) : base(context) { }

        public IEnumerable<UserDto> Execute(UserSearch request)
        {
            var query = Context.Users
                .Include(u => u.Role)
                .Include(u => u.Articles)
                .AsQueryable();

            if (request.FirstName != null)
                query = query.Where(u => u.FirstName.ToLower().Contains(request.FirstName.ToLower()));

            if (request.LastName != null)
                query = query.Where(u => u.LastName.ToLower().Contains(request.LastName.ToLower()));

            if (request.Email != null)
                query = query.Where(u => u.Email.ToLower().Contains(request.Email.ToLower()));

            if (request.IsAdmin == true)
                query = query.Where(u => u.Role.Name.ToLower().Contains("admin")); // or u.RoleId

            return query.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.LastName,
                Role = u.Role.Name,
                Articles = u.Articles.Select(a => a.Title)
            });
        }
    }
}