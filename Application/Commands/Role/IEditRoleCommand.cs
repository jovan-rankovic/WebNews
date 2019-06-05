using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Role
{
    public interface IEditRoleCommand : ICommand<(int id, RoleDto roleDto)> { }
}