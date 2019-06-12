using Application.DataTransfer;
using Application.Interfaces;
using Application.Responses;
using Application.Searches;

namespace Application.Commands.Role
{
    public interface IGetRolesCommand : ICommand<RoleSearch, PagedResponse<RoleDto>> { }
}