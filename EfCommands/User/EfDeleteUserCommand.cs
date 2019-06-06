using Application.Commands.User;
using Application.Exceptions;
using EfDataAccess;

namespace EfCommands.User
{
    public class EfDeleteUserCommand : BaseEfCommand, IDeleteUserCommand
    {
        public EfDeleteUserCommand(WebNewsContext context) : base(context) { }

        public void Execute(int request)
        {
            var user = Context.Users.Find(request);

            if (user == null)
                throw new EntityNotFoundException("User");

            Context.Users.Remove(user);

            Context.SaveChanges();
        }
    }
}