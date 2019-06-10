using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Comment
{
    public interface ICreateCommentCommand : ICommand<CommentDto> { }
}