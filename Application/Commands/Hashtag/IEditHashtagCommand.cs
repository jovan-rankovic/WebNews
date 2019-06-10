using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Hashtag
{
    public interface IEditHashtagCommand : ICommand<(int id, HashtagDto hashtagDto)> { }
}