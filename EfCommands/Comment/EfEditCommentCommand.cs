using Application.Commands.Comment;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;

namespace EfCommands.Comment
{
    public class EfEditCommentCommand : BaseEfCommand, IEditCommentCommand
    {
        public EfEditCommentCommand(WebNewsContext context) : base(context) { }

        public void Execute((int id, CommentDto commentDto) request)
        {
            var comment = Context.Comments.Find(request.id);

            if (comment == null)
                throw new EntityNotFoundException("Comment");

            comment.UpdatedAt = System.DateTime.Now;
            comment.Text = request.commentDto.Text.Trim();
            comment.UserId = request.commentDto.UserId;
            comment.ArticleId = request.commentDto.ArticleId;

            Context.SaveChanges();
        }
    }
}