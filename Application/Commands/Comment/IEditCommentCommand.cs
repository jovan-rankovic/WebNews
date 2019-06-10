using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Comment
{
    public interface IEditCommentCommand : ICommand<(int id, CommentDto commentDto)> { }
}