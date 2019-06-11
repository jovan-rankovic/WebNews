using Application.Commands.User;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using System.Linq;

namespace EfCommands.User
{
    public class EfCreateUserCommand : BaseEfCommand, ICreateUserCommand
    {
        public EfCreateUserCommand(WebNewsContext context) : base(context) { }

        public void Execute(UserDto request)
        {
            if (Context.Users.Any(u => u.Email == request.Email.Trim()))
                throw new EntityAlreadyExistsException("Email");

            Context.Users.Add(new Domain.User
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email.Trim(),
                Password = request.Password,
                RoleId = request.RoleId
            });

            Context.SaveChanges();
        }
    }
}