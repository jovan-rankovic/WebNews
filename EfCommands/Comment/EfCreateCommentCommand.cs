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
                Text = request.Text,
                ArticleId = 1, // testing
                UserId = 1 // testing
            });

            Context.SaveChanges();
        }
    }
}