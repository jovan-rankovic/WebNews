using Application.DataTransfer;
using Application.Interfaces;
using Application.Searches;
using System.Collections.Generic;

namespace Application.Commands.User
{
    public interface IGetUsersCommand : ICommand<UserSearch, IEnumerable<UserDto>> { }
}