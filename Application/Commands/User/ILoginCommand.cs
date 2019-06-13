using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.User
{
    public interface ILoginCommand : ICommand<(string email, string password), UserDto> { }
}