using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.User
{
    public interface IGetUserCommand : ICommand<int, UserDto> { }
}