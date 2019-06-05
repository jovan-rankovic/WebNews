using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Role
{
    public interface IGetRoleCommand : ICommand<int, RoleDto> { }
}