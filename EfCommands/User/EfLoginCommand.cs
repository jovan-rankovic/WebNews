using Application.Commands.User;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EfCommands.User
{
    public class EfLoginCommand : BaseEfCommand, ILoginCommand
    {
        public EfLoginCommand(WebNewsContext context) : base(context) { }

        public UserDto Execute((string email, string password) request)
        {
            var user = Context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == request.email && u.Password == request.password);

            if (user == null)
                throw new LoginFailedException("Invalid credentials.");

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.Name
            };
        }
    }
}