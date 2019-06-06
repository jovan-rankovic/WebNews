using Application.Commands.Role;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Role
{
    public class EfEditRoleCommand : BaseEfCommand, IEditRoleCommand
    {
        public EfEditRoleCommand(WebNewsContext context) : base(context) { }

        public void Execute((int id, RoleDto roleDto) request)
        {
            var role = Context.Roles.Find(request.id);

            if (role == null)
                throw new EntityNotFoundException("Role");

            if (role.Name != request.roleDto.Name)
                if (Context.Roles.Any(r => r.Name == request.roleDto.Name))
                    throw new EntityAlreadyExistsException("Role");

            role.UpdatedAt = System.DateTime.Now;
            role.Name = request.roleDto.Name;

            Context.SaveChanges();
        }
    }
}