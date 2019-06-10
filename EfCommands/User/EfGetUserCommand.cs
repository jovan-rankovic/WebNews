using Application.Commands.User;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EfCommands.User
{
    public class EfGetUserCommand : BaseEfCommand, IGetUserCommand
    {
        public EfGetUserCommand(WebNewsContext context) : base(context) { }

        public UserDto Execute(int request)
        {
            var user = Context.Users
                .Include(u => u.Role)
                .Include(u => u.Articles)
                .First(u => u.Id == request);

            if (user == null)
                throw new EntityNotFoundException("User");

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleId = user.RoleId,
                Role = user.Role.Name,
                Articles = user.Articles.Select(a => a.Title),
                Comments = user.Comments.Select(c => c.Text)
            };
        }
    }
}