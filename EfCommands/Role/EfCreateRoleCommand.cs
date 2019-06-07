using Application.Commands.Role;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Role
{
    public class EfCreateRoleCommand : BaseEfCommand, ICreateRoleCommand
    {
        public EfCreateRoleCommand(WebNewsContext context) : base(context) { }

        public void Execute(RoleDto request)
        {
            if (Context.Roles.Any(r => r.Name == request.Name.Trim()))
                throw new EntityAlreadyExistsException("Role");

            Context.Roles.Add(new Domain.Role
            {
                Name = request.Name.Trim()
            });

            Context.SaveChanges();
        }
    }
}