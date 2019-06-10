using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Hashtag
{
    public interface ICreateHashtagCommand : ICommand<HashtagDto> { }
}