using Application.Commands.User;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using System.Linq;

namespace EfCommands.User
{
    public class EfEditUserCommand : BaseEfCommand, IEditUserCommand
    {
        public EfEditUserCommand(WebNewsContext context) : base(context) { }

        public void Execute((int id, UserDto userDto) request)
        {
            var user = Context.Users.Find(request.id);

            if (user == null)
                throw new EntityNotFoundException("User");

            if (user.Email != request.userDto.Email)
                if (Context.Users.Any(u => u.Email == request.userDto.Email))
                    throw new EntityAlreadyExistsException("Email");

            user.UpdatedAt = System.DateTime.Now;
            user.FirstName = request.userDto.FirstName;
            user.LastName = request.userDto.LastName;
            user.Email = request.userDto.Email;

            Context.SaveChanges();
        }
    }
}