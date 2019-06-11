using Application.Commands.Comment;
using Application.DataTransfer;
using EfDataAccess;

namespace EfCommands.Comment
{
    public class EfCreateCommentCommand : BaseEfCommand, ICreateCommentCommand
    {
        public EfCreateCommentCommand(WebNewsContext context) : base(context) { }

        public void Execute(CommentDto request)
        {
            Context.Comments.Add(new Domain.Comment
            {
                Text = request.Text.Trim(),
                ArticleId = request.ArticleId,
                UserId = request.UserId
            });

            Context.SaveChanges();
        }
    }
}