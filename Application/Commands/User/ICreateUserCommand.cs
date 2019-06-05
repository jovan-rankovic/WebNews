using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.User
{
    public interface ICreateUserCommand : ICommand<UserDto> { }
}