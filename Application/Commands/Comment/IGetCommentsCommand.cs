using Application.DataTransfer;
using Application.Interfaces;
using Application.Responses;
using Application.Searches;

namespace Application.Commands.Comment
{
    public interface IGetCommentsCommand : ICommand<CommentSearch, PagedResponse<CommentDto>> { }
}