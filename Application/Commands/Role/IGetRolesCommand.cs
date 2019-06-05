using Application.DataTransfer;
using Application.Interfaces;
using Application.Searches;
using System.Collections.Generic;

namespace Application.Commands.Role
{
    public interface IGetRolesCommand : ICommand<RoleSearch, IEnumerable<RoleDto>> { }
}