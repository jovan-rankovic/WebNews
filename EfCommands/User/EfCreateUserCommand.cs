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
            if (Context.Users.Any(u => u.Email == request.Email))
                throw new EntityAlreadyExistsException("Email");

            Context.Users.Add(new Domain.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password
            });

            Context.SaveChanges();
        }
    }
}