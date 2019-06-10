using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Hashtag
{
    public interface IGetHashtagCommand : ICommand<int, HashtagDto> { }
}