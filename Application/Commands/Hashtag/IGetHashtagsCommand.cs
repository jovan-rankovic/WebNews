using Application.DataTransfer;
using Application.Interfaces;
using Application.Responses;
using Application.Searches;

namespace Application.Commands.Hashtag
{
    public interface IGetHashtagsCommand : ICommand<HashtagSearch, PagedResponse<HashtagDto>> { }
}