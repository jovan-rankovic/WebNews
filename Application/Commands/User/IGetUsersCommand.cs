using Application.DataTransfer;
using Application.Interfaces;
using Application.Responses;
using Application.Searches;

namespace Application.Commands.User
{
    public interface IGetUsersCommand : ICommand<UserSearch, PagedResponse<UserDto>> { }
}