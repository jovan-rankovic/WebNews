using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.User
{
    public interface IEditUserCommand : ICommand<(int id, UserDto userDto)> { }
}